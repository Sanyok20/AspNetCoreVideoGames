using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VideoGames.BLL.Settings;
using VideoGames.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VideoGames.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;

            if (string.IsNullOrEmpty(_settings.SecretKey))
            {
                throw new ArgumentNullException("Jwt secret key is null");
            }
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var bytes = Encoding.UTF8.GetBytes(_settings.SecretKey);
            var key = new SymmetricSecurityKey(bytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddHours(_settings.ExpiresHours)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateAccessToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(_settings.SecretKey);
            var key = new SymmetricSecurityKey(bytes);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetUserId(string token)
        {
            bool isValid = ValidateAccessToken(token);

            if (!isValid)
            {
                throw new SecurityTokenArgumentException("Невалідний токен");
            }

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                throw new SecurityTokenArgumentException("Невалідний токен");
            }

            var jwt = handler.ReadJwtToken(token);

            var idValue = jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value
                ?? throw new SecurityTokenArgumentException("Claim 'userId' не знайдено");

            bool parseResult = int.TryParse(idValue, out int userId);

            return parseResult ? userId : throw new FormatException("UserId incorrect");
        }
    }
}