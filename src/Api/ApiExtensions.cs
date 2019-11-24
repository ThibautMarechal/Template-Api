using System.Text;
using Api.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository;

namespace Api
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, AuthConfiguration authConfiguration)
        {
            //Authorizations
            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<TemplateContext>()
                .AddDefaultTokenProviders();
            
            //Authentications
            var key = Encoding.ASCII.GetBytes(authConfiguration.Secret);
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return services;
        }
    }
}