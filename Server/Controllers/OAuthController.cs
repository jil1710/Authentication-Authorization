using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        private readonly IConfiguration configuration;

        public OAuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Authorize(string response_type,string client_id,string redirect_uri, string scope,string state)
        {
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);
            return View(model:query.ToString());
        }


        [HttpPost]
        public IActionResult Authorize(string username, string redirectUri, string state) 
        {
            const string code = "JILPATEL";
            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);
            return Redirect($"{redirectUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(string grant_type,string code,string redirect_uri, string client_id)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Jil Patel"),
                        new Claim(ClaimTypes.Role, "Administrator"),
                        new Claim("IsDisable", "False")
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));

            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["JWT:Issuer"], configuration["JWT:Audience"], claims, DateTime.Now, DateTime.Now.AddHours(1), signInCred);

            var jsonToken = new JwtSecurityTokenHandler().WriteToken(token);

            var responseType = new
            {
                access_token = jsonToken,
                token_type = "Bearer",
                raw_claim = "PATEL"
            };

            var resJson = JsonConvert.SerializeObject(responseType);

            var resByte = Encoding.UTF8.GetBytes(resJson);  

            await Response.Body.WriteAsync(resByte,0,resByte.Length);   

            return Redirect(redirect_uri); 
        }

    }
}
