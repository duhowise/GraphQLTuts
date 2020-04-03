using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels.Schema;

namespace PizzaOrder.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContext<PizzaDbContext>(optionsAction:options=>options.UseSqlServer(Configuration.GetConnectionString("PizzaOrder")),contextLifetime:ServiceLifetime.Singleton);
            services.AddCustomJwtAuth(Configuration);
            services.AddCustomGraphqlAuth();
            services.AddPizzaAndOrderDetailServices();
            services.AddCustomGraphQlServices();
            services.AddCustomGraphQlTypes();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,PizzaDbContext dbContext)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            dbContext.EnsureDataSeeded();
            app.UseWebSockets();
            app.UseGraphQL<PizzaOrderSchema>();
            app.UseGraphQLWebSockets<PizzaOrderSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions());
        }
    }
}
