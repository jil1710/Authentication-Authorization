
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CustomAuthorizeAttribute.Helpers
{
    public class RoleRequiredAttribute : TypeFilterAttribute
    {
        public RoleRequiredAttribute(params string[] claim) : base(typeof(RoleFilter))
        {
            Arguments = new object[] { claim };
        }

    }

    public class RoleFilter : IAuthorizationFilter
    {
        readonly string[] _claim;

        public RoleFilter(params string[] claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (IsAuthenticated)
            {
                bool flagClaim = false;
                foreach (var item in _claim)
                {
                    if (context.HttpContext.User.HasClaim(ClaimTypes.Role, item))
                        flagClaim = true;
                }
                if (!flagClaim)
                    context.Result = new UnauthorizedObjectResult("Access Denied!");
            }
            else
            {
                context.Result = new JsonResult("UnAuthenticate To Access This Resource");
            }
            return;
        }
    }
}
