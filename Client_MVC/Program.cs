using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace Client_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";

            }).AddCookie("Cookie")
            .AddOpenIdConnect("oidc",config =>
            {
                config.SignInScheme = "Cookie";
                config.Authority = "https://localhost:44349/";

                config.ClientId = "client_id_mvc";

                config.ClientSecret = "client_secret_mvc";

                config.SaveTokens = true;

                config.ResponseType = "code";

                config.Scope.Add("RoleClaim");

                

            });

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}