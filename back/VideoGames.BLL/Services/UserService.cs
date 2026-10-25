using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.User;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        public UserService(UserRepository userRepository, JwtService jwtService, EmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<ResponseDto> AddToRoleAsync(int userId, string roleName, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return ResponseDto.Error($"Користувача з id '{userId}' не знайдено");
            }

            await _userRepository.AddToRoleAsync(user, roleName, ct);

            return ResponseDto.Success($"Користувачу додано роль '{roleName}'");
        }

        public async Task<ResponseDto> RemoveFromRoleAsync(int userId, string roleName, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return ResponseDto.Error($"Користувача з id '{userId}' не знайдено");
            }

            await _userRepository.DeleteFromRoleAsync(user, roleName, ct);

            return ResponseDto.Success($"Користувача видалено з ролі '{roleName}'");
        }

        public async Task<ResponseDto> SendEmailConfrimMessageAsync(string token, ChangeEmailDto? dto = null, TokenType tokenType = TokenType.ConfirmEmail, CancellationToken ct = default)
        {
            int userId = _jwtService.GetUserId(token);

            var user = await _userRepository.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return ResponseDto.Error($"Користувача з id '{userId}' не знайдено");
            }

            string emailToken = await _jwtService.GenerateEmailConfirmTokenAsync(user, newEmail: dto?.Email, type: tokenType, ct: ct);

            switch (tokenType)
            {
                case TokenType.ConfirmEmail:
                    await _emailService.SendEmailConfirmMessageAsync(user, emailToken, ct: ct);
                    break;
                case TokenType.ChangeEmail:
                    await _emailService.SendEmailChangeMessageAsync(user, dto!.Email, emailToken, ct: ct);
                    break;
            }


            return ResponseDto.Success("Лист відправлено");
        }

        public async Task<ResponseDto> ChangeEmailAsync(ChangeEmailDto dto, string jwtToken, CancellationToken ct = default)
        {
            var userId = _jwtService.GetUserId(jwtToken);

            var user = await _userRepository.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return ResponseDto.Error($"Користувача з id '{userId}' не знайдено");
            }

            await _userRepository.ChangeEmailAsync(user, dto.Email, ct);

            return ResponseDto.Success("Пошту змінено");
        }

        public async Task<ResponseDto> ConfirmEmailAsync(int userId, string token, CancellationToken ct = default)
        {
            return await _jwtService.ConfirmEmailAsync(userId, token, ct);
        }
    }
}