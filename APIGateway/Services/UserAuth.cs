using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Services
{
    public static class UserAuth
    {
        public static IServiceCollection UserToken(this IServiceCollection service, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"));

            var tokenValidationsParam = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidIssuer = "Admin",
                ValidAudience = "user",
                RequireExpirationTime = true
            };
            service.AddAuthentication().AddJwtBearer("UserToken", x => {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationsParam;
            });

            return service;
                
        }
    }
}
