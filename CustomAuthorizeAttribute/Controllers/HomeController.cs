using AuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using AuthenticationAndAuthorization.Dataset;
using CustomAuthorizeAttribute.Helpers;

namespace CustomAuthorizeAttribute.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> Login([FromBody] LoginModel login)
        {
            var user = UserContext.DB().FirstOrDefault(x => x.UserName == login.UserName && x.Password == login.Password && x.IsDisable == false);

            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, "BANK_MANAGER"),
                        new Claim("IsDisable", user.IsDisable.ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Results.Ok("Login Successfull");
            }

            return Results.BadRequest("Invalid Login Attempt");
        }


        [RoleRequired("BANK_MANAGER")]
        [HttpGet]
        public IResult Secret()
        {
            return Results.Ok("Access Granted");
        }


        [HttpGet]
        public async Task<IResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok("Logout Successfull");
        }
    }
}
