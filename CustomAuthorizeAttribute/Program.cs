
using AuthenticationAndAuthorization.Dataset;
using AuthenticationAndAuthorization.Events;
using AuthenticationAndAuthorization.Models;
using CustomAuthorizeAttribute.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomAuthorizeAttribute
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.AccessDeniedPath = "/access-denied";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                option.EventsType = typeof(CustomCookieAuthenticationEvent);
            });
            builder.Services.AddScoped<CustomCookieAuthenticationEvent>();
            builder.Services.AddAuthorization();

            //builder.Services.AddTransient<RoleRequiredAttribute>();

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


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}