namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "ClientCookieAuthentication";
                option.DefaultSignInScheme = "ClientCookieAuthentication";
                option.DefaultChallengeScheme = "ServerAuthentication";
            }).AddCookie("ClientCookieAuthentication")
            .AddOAuth("ServerAuthentication", config =>
            {
                config.CallbackPath = "/oauth/callback";
                config.ClientId = "client_id";
                config.ClientSecret = "client_secret";
                config.AuthorizationEndpoint = "https://localhost:44382/oauth/authorize";
                config.TokenEndpoint = "https://localhost:44382/oauth/token";
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