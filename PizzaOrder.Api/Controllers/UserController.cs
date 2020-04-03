using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PizzaOrder.Api.Models;
using static PizzaOrder.Business.Helpers.Constants;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PizzaOrder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(IConfiguration configuration,SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,RoleManager<IdentityRole>roleManager,IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
        }

       [AllowAnonymous] public async Task<IActionResult> Authenticate([FromBody] LoginDetails loginDetails)
       {
           var user = await _userManager.FindByNameAsync(loginDetails.UserName);
           if (user==null)
           {
               return NotFound();
           }

           var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDetails.Password, true);
           if (signInResult.Succeeded)
           {
               var token = await GetJwtTokenAsync(user);
           return Ok(token);
           }

           return BadRequest(new {message = "username or password incorrect"});
       }
       
       [AllowAnonymous] public async Task<IActionResult> CreateDefaultUsers()
        {
            var roleDetails=new List<string>
            {
                Roles.Customer,
                Roles.Restaurant,
                Roles.Admin
            };
            foreach (var roleDetail in roleDetails)
            {
                if (!await _roleManager.RoleExistsAsync(roleDetail))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleDetail));
                }
            }

            var userDetails=new Dictionary<string,IdentityUser>
            {
                {Roles.Customer,new IdentityUser{Email = "customer@demo.com",UserName = "Customer",EmailConfirmed = true,} },
                { Roles.Restaurant,new IdentityUser{Email = "Restaurant@demo.com",UserName = "Restaurant",EmailConfirmed = true,} },
                {Roles.Admin,new IdentityUser{Email = "Admin@demo.com",UserName = "Admin",EmailConfirmed = true,} }
            };

            foreach (var identityUser in userDetails)
            {
                await _userManager.CreateAsync(identityUser.Value, "qwe123");
            }

            return Ok("successfully created default users");

        }
       
        public async Task<IActionResult> ProtectedPage()
        {
            var identity = _contextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var user =await _userManager.FindByNameAsync(userName);
            return Ok(user);


        }
        public async Task<IActionResult> GetJwtTokenAsync(IdentityUser user)
        {
            var keyInBytes =
                System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtIssuerOptions:SecretKey").Value);
            var signingCredentials=new SigningCredentials(new SymmetricSecurityKey(keyInBytes),SecurityAlgorithms.Sha256);
            var userRoles = await _userManager.GetRolesAsync(user);
            var tokenClaims=new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.NameIdentifier,user.Id),


            };
            var roleClaims = userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)).ToList();
            tokenClaims.AddRange(roleClaims);

            var tokenExpiration=DateTime.UtcNow.AddDays(1);
            var token=new JwtSecurityToken(issuer:_configuration.GetSection("JwtIssuerOptions:Issuer").Value,
                audience:_configuration.GetSection("JwtIssuerOptions:Audience").Value,
                claims:tokenClaims,
                signingCredentials:signingCredentials,
                expires:tokenExpiration
            );
            var tokenDetails=new TokenDetails
            {
                UserId = user.Id,
                ExpireOn = tokenExpiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            var currentUser =await  _userManager.FindByEmailAsync(user.Email);
            var existingClaims =await _userManager.GetClaimsAsync(currentUser);
            await _userManager.RemoveClaimsAsync(currentUser, existingClaims);
            await _userManager.AddClaimsAsync(currentUser, tokenClaims);
            return Ok(tokenDetails);

        }
    }
}