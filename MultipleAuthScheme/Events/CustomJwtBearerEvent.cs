using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthenticationAndAuthorization.Dataset;

namespace MultipleAuthScheme.Events
{
    public class CustomJwtBearerEvent : JwtBearerEvents
    {
        public override Task TokenValidated(TokenValidatedContext context)
        {

            var userPrincipal = context.Principal;

            var IsDisable = (from c in userPrincipal.Claims
                             where c.Type == "IsDisable"
                             select c.Value).FirstOrDefault();

            var user = UserContext.DB().FirstOrDefault(x => x.UserName == userPrincipal.Identity!.Name && x.IsDisable == true);

            if (user != null)
            {
                if (!string.IsNullOrEmpty(IsDisable))
                {
                    context.Fail("Access Denied!...");  
                }
            }

            return Task.CompletedTask;
        }
    }
}
