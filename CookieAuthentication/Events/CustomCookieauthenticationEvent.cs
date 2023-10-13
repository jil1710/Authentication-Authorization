using AuthenticationAndAuthorization.Dataset;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthenticationAndAuthorization.Events
{
    public class CustomCookieAuthenticationEvent : CookieAuthenticationEvents
    {

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            var IsDisable = (from c in userPrincipal.Claims
                             where c.Type == "IsDisable"
                             select c.Value).FirstOrDefault();

            var user = UserContext.DB().FirstOrDefault(x => x.UserName == userPrincipal.Identity!.Name && x.IsDisable == true);

            if(user != null)
            {
                if (!string.IsNullOrEmpty(IsDisable))
                {
                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }

            
        }
    }
}
