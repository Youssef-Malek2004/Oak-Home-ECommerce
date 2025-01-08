using System.Security.Cryptography;
using Inventory.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Payments.Api.OptionsSetup;

public class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public void Configure(JwtBearerOptions options)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportFromPem(_jwtOptions.PublicKey);
        
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
            IssuerSigningKey = new RsaSecurityKey(rsa),
        };
    }

    public void Configure(string? name, JwtBearerOptions options) => Configure(options);
}