using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WorkoutApi.Services
{
    public class JwtHelper : IJwtHelper
    {
        /// <inheritdoc />
        public Guid? ExtractUserKey(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRETKEY"));

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                var userKeyClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "userKey");
                if (userKeyClaim != null && Guid.TryParse(userKeyClaim.Value, out var userKey))
                {
                    return userKey;
                }

                return null;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while extracting user key from token", ex);
            }
        }

        /// <inheritdoc />
        public string GenerateAccessToken(Guid? userKey)
        {
            if (userKey == null) throw new ArgumentNullException(nameof(userKey));

            var claims = new List<Claim>
            {
                new Claim("userKey", userKey.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER"),
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRETKEY"))),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
