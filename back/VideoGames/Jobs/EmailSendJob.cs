using Microsoft.EntityFrameworkCore;
using Quartz;
using VideoGames.BLL.Services;
using VideoGames.DAL.Repositories;

namespace VideoGames.Jobs
{
    public class EmailSendJob : IJob
    {
        private readonly UserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly IWebHostEnvironment _env;

        public EmailSendJob(UserRepository userRepository, EmailService emailService, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _env = env;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = await _userRepository.Users.Where(u => u.IsSubscribe).ToListAsync();

            var root = _env.ContentRootPath;
            var templatePath = Path.Combine(root, "FileStorage", "templates", "newYear.html");
            var html = await File.ReadAllTextAsync(templatePath);

            await _emailService.SendEmailAsync(users.Select(u => u.Email), "З Новим Роком!", html);
        }
    }
}