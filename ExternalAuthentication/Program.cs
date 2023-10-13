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

                option.ClientId = "1050251688234-paaoghhlh1hg9e8h85kj7iei0a8k2b9d.apps.googleusercontent.com";
                option.ClientSecret = "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";
            }).AddGitHub(option =>
            {
                option.ClientId = "27c7959559c86d9bd134";
                option.ClientSecret = "027eeb92c6f1e64a5441d9781dc9edb52b671930";
                option.Scope.Add("user:email");

            }).AddFacebook(option =>
            {
                option.AppId = "251560094320402";
                option.AppSecret = "3cbe9d3d8e76489e0821e4c47c102321";
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