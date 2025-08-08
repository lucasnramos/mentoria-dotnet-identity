using Authentication.Adapter.Configurations;
using Authentication.Adapter.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Authentication.Adapter.Token
{
    public static class GenerateToken
    {
        const int tokenHours = 2;
        public static TokenModel GetToken(Guid id,
                                        string email,
                                        string name,
                                        int role,
                                        string tokenIssuer,
                                        string tokenAudience,
                                        SigningConfigurations signingConfigurations)
        {
            var userRole = role switch
            {
                1 => "Admin",
                2 => "User",
                _ => "Guest"
            };

            var identity = new ClaimsIdentity(
                new GenericIdentity(email!, "Email"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, name!),
                        new Claim(ClaimTypes.Role, userRole),
                        new Claim(JwtRegisteredClaimNames.Email, email!)
                }
            );

            var dateCreated = DateTime.Now;
            var dateExpiration = dateCreated + TimeSpan.FromHours(tokenHours);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenIssuer,
                Audience = tokenAudience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dateCreated,
                Expires = dateExpiration
            });

            var token = handler.WriteToken(securityToken);

            return new()
            {
                Authenticated = true,
                Created = dateCreated.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dateExpiration.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "OK"
            };
        }
    }
}