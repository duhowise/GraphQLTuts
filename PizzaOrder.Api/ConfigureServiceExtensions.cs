using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Server;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using PizzaOrder.Business.Helpers;
using PizzaOrder.Business.Services;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels;
using PizzaOrder.GraphQlModels.Enums;
using PizzaOrder.GraphQlModels.InputTypes;
using PizzaOrder.GraphQlModels.Mutations;
using PizzaOrder.GraphQlModels.Queries;
using PizzaOrder.GraphQlModels.Schema;
using PizzaOrder.GraphQlModels.Subscriptions;
using PizzaOrder.GraphQlModels.Types;

namespace PizzaOrder.Api
{
    public static class ConfigureServiceExtensions
    {
        public static void AddPizzaAndOrderDetailServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPizzaDetailService, PizzaDetailService>();
            serviceCollection.AddTransient<IOrderDetailService, OrderDetailService>();
            serviceCollection.AddTransient<IEventService, EventService>();
        }


        public static void AddCustomGraphQlServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IServiceProvider>(x =>
                new FuncServiceProvider(x.GetRequiredService));
            serviceCollection.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.ExposeExceptions = false;
                    options.UnhandledExceptionDelegate = context =>
                    {
                        Console.WriteLine($"Error {context.OriginalException.Message}");
                    };
                })
                .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { }).AddUserContextBuilder(
                    httpContext =>
                  new GraphQlUserContext
                  {
                      User = httpContext.User
                    }).AddWebSockets()
                .AddDataLoader().AddGraphTypes(typeof(PizzaOrderSchema));
        }

        public static void AddCustomGraphQlTypes(this IServiceCollection services)
        {
            services.AddSingleton<OrderDetailsType>();
            services.AddSingleton<PizzaDetailsType>();
            services.AddSingleton<EventDataType>();

            services.AddSingleton<OrderStatusEnumType>();
            services.AddSingleton<ToppingsEnumType>();
            services.AddSingleton<CompletedOrdersSortingFieldEnumType>();
            services.AddSingleton<SortingDirectionEnumType>();

            services.AddSingleton<OrderDetailsInputType>();
            services.AddSingleton<PizzaDetailsInputType>();
            services.AddSingleton<CompletedOrderOrderByInputType>();


            services.AddSingleton<PizzaOrderQuery>();
            services.AddSingleton<PizzaOrderSchema>();
            services.AddSingleton<PizzaOrderMutation>();
            services.AddSingleton<PizzaOrderSubscription>();

        }

        public static void AddCustomGraphqlAuth(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator,AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
            services.TryAddSingleton(_ =>
            {
                var authSettings=new AuthorizationSettings();
                authSettings.AddPolicy(Constants.AuthPolicy.CustomerPolicy,policy=>policy.RequireClaim(ClaimTypes.Role,Constants.Roles.Customer));
                
                authSettings.AddPolicy(Constants.AuthPolicy.RestaurantPolicy,policy=>policy.RequireClaim(ClaimTypes.Role,Constants.Roles.Restaurant));
   authSettings.AddPolicy(Constants.AuthPolicy.AdminPolicy,policy=>policy.RequireClaim(ClaimTypes.Role,Constants.Roles.Restaurant,Constants.Roles.Restaurant,Constants.Roles.Admin));

                return authSettings;
            });
        }
        public static void AddCustomJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var signinKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["JwtIssuerOptions:SecretKey"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PizzaDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz";
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetSection("JwtIssuerOptions:Issuer").Value,
                        ValidAudience = configuration.GetSection("JwtIssuerOptions:Audience").Value,
                        ValidateLifetime = true,
                        IssuerSigningKey = signinKey,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}