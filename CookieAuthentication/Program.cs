
using AuthenticationAndAuthorization.Dataset;
using AuthenticationAndAuthorization.Events;
using AuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationAndAuthorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.AccessDeniedPath = "/access-denied";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                option.EventsType = typeof(CustomCookieAuthenticationEvent);
            });

            builder.Services.AddScoped<CustomCookieAuthenticationEvent>();

            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("Secret", p =>
                {
                    p.RequireRole("Administrator");
                });
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            

            app.MapPost("/login", async ([FromBody]LoginModel login, HttpContext httpContext) =>
            {
                var user = UserContext.DB().FirstOrDefault(x=> x.UserName == login.UserName && x.Password == login.Password && x.IsDisable==false);

                if(user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, "Administrator"),
                        new Claim("IsDisable", user.IsDisable.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                        
                    };

                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

                    return Results.Ok("Login Successfull");
                }

                return Results.BadRequest("Invalid Login Attempt");
            });

            
            // Protected Route
            app.MapGet("/secret", (HttpContext httpContext) =>
            {
                return Results.Ok($"Hello User {httpContext.User.Identity!.Name}");
            }).RequireAuthorization("Secret");


            app.MapGet("/access-denied", () =>
            {
                return Results.Unauthorized();
            });


            app.MapGet("/logout", async (HttpContext httpContext) =>
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok("Logout Successfully");
            });
            

            app.Run();
        }
    }
}