using ExternalAuthentication.Areas.Identity.Data;
using ExternalAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ExternalAuthentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("IdentityDataContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDataContextConnection' not found.");

            builder.Services.AddDbContext<IdentityDataContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<OAuth_IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<IdentityDataContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication().AddGoogle("google", option =>
            {

                option.ClientId = "xxxxxxxxxx";
                option.ClientSecret = "xxxxxxxx";
            }).AddGitHub(option =>
            {
                option.ClientId = "xxxxxxxx";
                option.ClientSecret = "xxxxxxxx";
                option.Scope.Add("user:email");

            }).AddFacebook(option =>
            {
                option.AppId = "xxxxxxxxxx";
                option.AppSecret = "xxxxxxxxxx";
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
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
