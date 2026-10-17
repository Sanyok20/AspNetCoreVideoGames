using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Settings;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _settings;
        private readonly UserTokenRepository _userTokenRepository;
        private readonly UserRepository _userRepository;

        public JwtService(IOptions<JwtSettings> options, UserTokenRepository userTokenRepository, UserRepository userRepository)
        {
            _settings = options.Value;

            if (string.IsNullOrEmpty(_settings.SecretKey))
            {
                throw new ArgumentNullException("Jwt secret key is null");
            }

            _userTokenRepository = userTokenRepository;
            _userRepository = userRepository;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("userName", user.UserName)
            };

            if (user.Roles.Count > 0)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim("role", role.Name));
                }
            }
            else
            {
                claims.Add(new Claim("role", "user"));
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

        public string GenerateSecureToken()
        {
            var randomBytes = new byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public async Task<string> GenerateEmailConfirmTokenAsync(User user, CancellationToken ct = default)
        {
            var secureToken = GenerateSecureToken();

            var token = new UserToken
            {
                Token = secureToken,
                User = user
            };

            await _userTokenRepository.CreateAsync(token, ct);

            return token.Token;
        }

        public async Task<ResponseDto> ConfirmEmailAsync(int userId, string token, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return ResponseDto.Error($"Користувач з id '{userId}' не знайдений");
            }

            var userToken = user.Tokens.FirstOrDefault(ut => ut.Token == token);

            if (userToken == null)
            {
                return ResponseDto.Error($"Невалідний токен");
            }

            if (userToken.Expires < DateTime.UtcNow)
            {
                await _userTokenRepository.DeleteAsync(userToken, ct);
                return ResponseDto.Error($"Невалідний токен");
            }

            user.EmailConfirmed = true;

            await _userTokenRepository.DeleteAsync(userToken, ct);

            await _userRepository.UpdateAsync(user);

            return ResponseDto.Success("Пошту підтверджено");
        }
    }
}