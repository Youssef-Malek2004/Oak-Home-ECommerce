using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Users.Application.Services;
using Users.Domain.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Users.Infrastructure.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };
        
        if (user.Roles != null)
        {
            var group = user.Roles.Any(role => role.Name == "Admin") ? "Admins" 
                : user.Roles.Any(role => role.Name == "Vendor") ? "Vendors" 
                : "Users";

            claims.Add(new Claim("custom_group", group));
            
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
        
                if (role.Permissions == null) continue;
                claims.AddRange(role.Permissions.Select(permission => 
                    new Claim("Permissions", permission.Name)));
            }
        }
        
        
        
        var claimsArray = claims.ToArray();
        
        var privateKey = new RSACryptoServiceProvider();
        privateKey.ImportFromPem(_jwtOptions.PrivateKey);
        
        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(privateKey),
            SecurityAlgorithms.RsaSha256);
        
        // var signingCredentials = new SigningCredentials(
        //     new SymmetricSecurityKey(
        //         Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
        //     SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claimsArray,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);
        
        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);
        
        return tokenValue;
    }
}