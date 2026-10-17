using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VideoGames.BLL.Controllers;
using VideoGames.BLL.Dtos.Auth;
using VideoGames.DAL;
using VideoGames.DAL.Entities;

namespace VideoGames.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync([userId], ct);
            if (user == null) return NotFound("Користувача не знайдено");

            await _context.SaveChangesAsync(ct);
            return Ok(new { message = "Пароль успішно змінено" });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email, ct);
            if (user == null) return Ok(new { message = "Якщо такий email існує, інструкцію надіслано." });

            string token = Guid.NewGuid().ToString();
            _context.UserTokens.Add(new UserToken { UserId = user.Id, Token = token });
            await _context.SaveChangesAsync(ct);

            await _emailService.SendPasswordResetMessageAsync(user, token, ct: ct);
            return Ok(new { message = "Лист для скидання пароля надіслано" });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromQuery] int uid, [FromBody] ResetPasswordDto dto, CancellationToken ct)
        {
            var userToken = await _context.UserTokens
                .FirstOrDefaultAsync(ut => ut.UserId == uid && ut.Token == dto.Token, ct);

            if (userToken == null) return BadRequest("Недійсний або прострочений токен");

            var user = await _context.Users.FindAsync([uid], ct);
            if (user == null) return NotFound("Користувача не знайдено");

            _context.UserTokens.Remove(userToken);
            await _context.SaveChangesAsync(ct);

            return Ok(new { message = "Пароль успішно скинуто" });
        }

        [HttpPost("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto, CancellationToken ct)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync([userId], ct);
            if (user == null) return NotFound("Користувача не знайдено");

            string token = Guid.NewGuid().ToString();
            _context.UserTokens.Add(new UserToken { UserId = user.Id, Token = token });
            await _context.SaveChangesAsync(ct);

            await _emailService.SendEmailChangeMessageAsync(user, dto.NewEmail, token, ct: ct);
            return Ok(new { message = "Лист для підтвердження надіслано на нову пошту" });
        }

        [HttpGet("confirm-email-change")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailChange([FromQuery] int uid, [FromQuery] string token, [FromQuery] string newEmail, CancellationToken ct)
        {
            var userToken = await _context.UserTokens
                .FirstOrDefaultAsync(ut => ut.UserId == uid && ut.Token == token, ct);

            if (userToken == null) return BadRequest("Недійсний токен");

            var user = await _context.Users.FindAsync([uid], ct);
            if (user == null) return NotFound("Користувача не знайдено");

            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpper();
            user.EmailConfirmed = true;

            _context.UserTokens.Remove(userToken);
            await _context.SaveChangesAsync(ct);

            return Ok(new { message = "Пошту успішно змінено!" });
        }
    }
}