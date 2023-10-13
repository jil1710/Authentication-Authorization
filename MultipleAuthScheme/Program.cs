
using AuthenticationAndAuthorization.Dataset;
using AuthenticationAndAuthorization.Events;
using AuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MultipleAuthScheme.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultipleAuthScheme
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<CustomCookieAuthenticationEvent>();
            builder.Services.AddScoped<CustomJwtBearerEvent>();

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                option.EventsType = typeof(CustomCookieAuthenticationEvent);

            }).AddJwtBearer(option =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!));
                option.EventsType = typeof(CustomJwtBearerEvent);
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = key
                };
            });

            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("JWT", p =>
                {
                    p.AuthenticationSchemes = new[] {JwtBearerDefaults.AuthenticationScheme};
                    p.RequireRole("Administrator");
                });

                option.AddPolicy("COOKIE", p =>
                {
                    p.AuthenticationSchemes = new[] { CookieAuthenticationDefaults.AuthenticationScheme };
                    p.RequireRole("Administrator");
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

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


            // Authenticate using JWT Token
            app.MapPost("/login-jwt", async ([FromBody] LoginModel login, IConfiguration configuration) =>
            {
                var user = UserContext.DB().FirstOrDefault(x => x.UserName == login.UserName && x.Password == login.Password && x.IsDisable == false);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, "Administrator"),
                        new Claim("IsDisable", user.IsDisable.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));

                    var signInCred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(configuration["JWT:Issuer"], configuration["JWT:Audience"],claims,DateTime.Now,DateTime.Now.AddHours(1),signInCred);

                    var jsonToken = new JwtSecurityTokenHandler().WriteToken(token);

                    return Results.Ok(new {Access_Token = jsonToken});
                }

                return Results.BadRequest("Invalid Login Attempt");
            });

            // Secret Only Access Using JWT Scheme
            app.MapGet("/jwt-secret", (HttpContext httpContext) =>
            {
                return Results.Ok($"Hello User {httpContext.User.Identity!.Name}");
            }).RequireAuthorization("JWT");


            // Authentication using Cookie Scheme
            app.MapPost("/login-cookie", async ([FromBody] LoginModel login, HttpContext httpContext) =>
            {
                var user = UserContext.DB().FirstOrDefault(x => x.UserName == login.UserName && x.Password == login.Password && x.IsDisable == false);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, "Administrator"),
                        new Claim("IsDisable", user.IsDisable.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),

                    };

                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return Results.Ok("Login Successfull");
                }

                return Results.BadRequest("Invalid Login Attempt");
            });


            // Secret Only Access Using Cookie auth
            app.MapGet("/cookie-secret", (HttpContext httpContext) =>
            {
                return Results.Ok($"Hello User {httpContext.User.Identity!.Name}");
            }).RequireAuthorization("COOKIE");





            app.Run();
        }
    }
}