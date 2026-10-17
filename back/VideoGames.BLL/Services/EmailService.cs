using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VideoGames.BLL.Settings;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.Controllers
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly SmtpClient _smtpClient;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _smtpSettings = options.Value;
            _smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
            _smtpClient.Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password);
            _smtpClient.EnableSsl = true;
        }

        public async Task SendEmailConfirmMessageAsync(User user, string token, string callBackUrl = "https://localhost:5000/api/user/confirm-email", CancellationToken ct = default)
        {
            string url = callBackUrl += $"?uid={user.Id}&token={token}";

            var root = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(root, "FileStorage", "templates", "emailConfirm.html");

            var html = await File.ReadAllTextAsync(templatePath, ct);

            html = html
                .Replace("{{UserName}}", user.UserName)
                .Replace("{{ConfirmationUrl}}", url);

            var message = new MailMessage();
            message.Subject = "Підтвердження пошти";
            message.To.Add(user.Email);
            message.From = new MailAddress(_smtpSettings.Email);
            message.Body = html;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message, ct);
        }

        public async Task SendPasswordResetMessageAsync(User user, string token, string callBackUrl = "https://localhost:5000/api/auth/reset-password", CancellationToken ct = default)
        {
            string url = $"{callBackUrl}?uid={user.Id}&token={token}";

            var root = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(root, "FileStorage", "templates", "resetPassword.html");

            string html = File.Exists(templatePath)
                ? await File.ReadAllTextAsync(templatePath, ct)
                : $"<p>Привіт, {{UserName}}! Перейди за посиланням для скидання пароля: <a href='{{ResetUrl}}'>Скинути пароль</a></p>";

            html = html
                .Replace("{{UserName}}", user.UserName)
                .Replace("{{ResetUrl}}", url);

            var message = new MailMessage();
            message.Subject = "Скидання пароля";
            message.To.Add(user.Email);
            message.From = new MailAddress(_smtpSettings.Email);
            message.Body = html;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message, ct);
        }

        public async Task SendEmailChangeMessageAsync(User user, string newEmail, string token, string callBackUrl = "https://localhost:5000/api/auth/confirm-email-change", CancellationToken ct = default)
        {
            string url = $"{callBackUrl}?uid={user.Id}&token={token}&newEmail={Uri.EscapeDataString(newEmail)}";

            var root = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(root, "FileStorage", "templates", "emailChange.html");

            string html = File.Exists(templatePath)
                ? await File.ReadAllTextAsync(templatePath, ct)
                : $"<p>Привіт, {{UserName}}! Підтверди зміну пошти за посиланням: <a href='{{ConfirmUrl}}'>Підтвердити</a></p>";

            html = html
                .Replace("{{UserName}}", user.UserName)
                .Replace("{{ConfirmUrl}}", url);

            var message = new MailMessage();
            message.Subject = "Підтвердження зміни пошти";
            message.To.Add(newEmail); 
            message.From = new MailAddress(_smtpSettings.Email);
            message.Body = html;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message, ct);
        }
    }
}