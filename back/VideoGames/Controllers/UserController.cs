using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.User;
using VideoGames.BLL.Services;
using VideoGames.DAL.Entities;
using VideoGames.Extensions;

namespace VideoGames.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IValidator<ChangeEmailDto> _changeEmailValidator;

        public UserController(UserService userService, IValidator<ChangeEmailDto> changeEmailValidator)
        {
            _userService = userService;
            _changeEmailValidator = changeEmailValidator;
        }

        [HttpPatch("add-role")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> AddToRoleAsync([FromQuery] int userId, [FromQuery] string roleName, CancellationToken ct = default)
        {
            var response = await _userService.AddToRoleAsync(userId, roleName, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPatch("remove-role")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveFromRole([FromQuery] int userId, [FromQuery] string roleName, CancellationToken ct = default)
        {
            var response = await _userService.RemoveFromRoleAsync(userId, roleName, ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("send-email-confirm-message")]
        [Authorize]
        public async Task<IActionResult> SendEmailConfrimMessageAsync(CancellationToken ct = default)
        {
            var token = this.GetJwtToken();

            if (token == null)
            {
                return Unauthorized(ResponseDto.Error("Вкажіть токен"));
            }

            var response = await _userService.SendEmailConfrimMessageAsync(token, ct: ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] int uid, [FromQuery] string token, CancellationToken ct = default)
        {
            var response = await _userService.ConfirmEmailAsync(uid, token, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPatch("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailDto dto, CancellationToken ct = default)
        {
            var validation = await _changeEmailValidator.ValidateAsync(dto, ct);

            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var token = this.GetJwtToken();

            if (token == null)
            {
                return Unauthorized(ResponseDto.Error("Вкажіть токен"));
            }

            var response = await _userService.SendEmailConfrimMessageAsync(token, dto, TokenType.ChangeEmail, ct: ct);
            return this.GetHttpResponse(response);
        }
    }
}