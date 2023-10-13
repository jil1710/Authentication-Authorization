using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Security.Claims;

namespace OpenIdServer
{
    public static class ServerConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "RoleClaim",
                UserClaims =
                {
                    ClaimTypes.Role
                }
            }
        };

        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource>()
        {
            new ApiResource("GetProduct")
        };

        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client()
            {
                ClientId = "client_id_mvc",
                ClientSecrets = {new Secret("client_secret_mvc".ToSha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:44372/signin-oidc" },
                AllowedScopes = { "GetProduct", IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,"RoleClaim" },
                AlwaysIncludeUserClaimsInIdToken = true,
                RequireConsent = false
            }
        };
    }
}
