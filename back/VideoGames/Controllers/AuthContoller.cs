using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.Auth;
using VideoGames.BLL.Services;
using VideoGames.Extensions;

namespace VideoGames.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly AuthService _authService;

        public AuthController(AuthService authService, IValidator<LoginDto> loginValidator, IValidator<RegisterDto> registerValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto, CancellationToken ct = default)
        {
            var validation = _loginValidator.Validate(dto);

            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var response = await _authService.LoginAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto, CancellationToken ct = default)
        {
            var validation = await _registerValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var response = await _authService.RegisterAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> ProfileAsync(CancellationToken ct = default)
        {
            var token = this.GetJwtToken();

            if (token == null)
            {
                return Unauthorized(ResponseDto.Error("Вкажіть токен"));
            }

            var response = await _authService.ProfileAsync(token, ct);
            return this.GetHttpResponse(response);
        }
    }
}