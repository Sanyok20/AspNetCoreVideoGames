using AutoMapper;
using VideoGames.BLL.Services;
using VideoGames.BLL.Controllers;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.Auth;
using VideoGames.BLL.Dtos.User;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(UserRepository userRepository, JwtService jwtService, IMapper mapper, EmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
        {
            var user =
                await _userRepository.GetByUserNameAsync(dto.Login, ct)
                ?? await _userRepository.GetByEmailAsync(dto.Login, ct);

            if (user == null)
            {
                return ResponseDto.Error($"Користувач з логіном '{dto.Login}' не знайдений");
            }

            bool passwordResult = _userRepository.CheckPassword(user, dto.Password);

            if (!passwordResult)
            {
                return ResponseDto.Error($"Невірний пароль");
            }

            var token = _jwtService.GenerateAccessToken(user);

            return ResponseDto.Success("Успішний вхід", token);
        }

        public async Task<ResponseDto> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(dto);

            await _userRepository.CreateAsync(user, dto.Password, ct);
            await _userRepository.AddToRoleAsync(user, "user", ct);

            var jwtToken = _jwtService.GenerateAccessToken(user);

            var emailToken = await _jwtService.GenerateEmailConfirmTokenAsync(user, ct);

            await _emailService.SendEmailConfirmMessageAsync(user, emailToken, ct: ct);

            return ResponseDto.Success("Успішна реєстрація", jwtToken);
        }

        public async Task<ResponseDto> ProfileAsync(string token, CancellationToken ct = default)
        {
            try
            {
                var userId = _jwtService.GetUserId(token);

                var user = await _userRepository.GetByIdAsync(userId, ct);

                if (user == null)
                {
                    return ResponseDto.Error($"Користувач з id '{userId}' не знайдений");
                }

                var dto = _mapper.Map<UserDto>(user);

                return ResponseDto.Success("Дані про користувача отримано", dto);
            }
            catch (Exception ex)
            {
                return ResponseDto.Error(ex.Message);
            }
        }
    }
}