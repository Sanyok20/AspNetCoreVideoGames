using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Services;
using VideoGames.Extensions;

namespace VideoGames.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
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
        public async Task<IActionResult> SendEmailConfrimMessageAsync([FromQuery] string token, CancellationToken ct = default)
        {
            var response = await _userService.SendEmailConfrimMessageAsync(token, ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] int uid, [FromQuery] string token, CancellationToken ct = default)
        {
            var response = await _userService.ConfirmEmailAsync(uid, token, ct);
            return this.GetHttpResponse(response);
        }
    }
}