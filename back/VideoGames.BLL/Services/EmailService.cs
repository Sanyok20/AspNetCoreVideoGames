using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VideoGames.BLL.Settings;
using VideoGames.DAL.Entities;

namespace VideoGames.BLL.Services
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

        public async Task SendEmailAsync(IEnumerable<string> to, string subject, string htmlBody, CancellationToken ct = default)
        {
            var message = new MailMessage();
            message.Subject = subject;
            message.From = new MailAddress(_smtpSettings.Email);
            message.Body = htmlBody;
            message.IsBodyHtml = true;
            foreach (var userEmail in to)
            {
                message.To.Add(userEmail);
            }

            await _smtpClient.SendMailAsync(message, ct);
        }

        public async Task SendEmailChangeMessageAsync(User user, string email, string token, string callBackUrl = "https://localhost:5000/api/user/confirm-email", CancellationToken ct = default)
        {
            string url = callBackUrl += $"?uid={user.Id}&token={token}";

            var root = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(root, "FileStorage", "templates", "changeEmail.html");

            var html = await File.ReadAllTextAsync(templatePath, ct);

            html = html
                .Replace("{{UserName}}", user.UserName)
                .Replace("{{ConfirmationUrl}}", url)
                .Replace("{{NewEmail}}", email);

            var message = new MailMessage();
            message.Subject = "Зміни пошти";
            message.To.Add(email);
            message.From = new MailAddress(_smtpSettings.Email);
            message.Body = html;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message, ct);
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

        public void SendBirthdayGreeting(User user, CancellationToken ct = default)
        {
            string subject = "З днем народження! 🎉";
            string message = $"Вітаємо, {user.FirstName ?? user.UserName}! Команда VideoGames щиро вітає Вас із днем народження! Бажаємо крутих ігор та перемог!";

            SendEmail(user.Email, subject, message, ct);
        }

        private void SendEmail(string email, string subject, string message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}