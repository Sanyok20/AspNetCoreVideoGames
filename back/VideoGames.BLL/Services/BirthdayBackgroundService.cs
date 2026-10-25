using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoGames.BLL.Services;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.BackgroundServices
{
    public class BirthdayBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BirthdayBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndSendBirthdaysAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckAndSendBirthdaysAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            var today = DateTime.Today;
            var users = await userRepository.GetUsersWithBirthdayTodayAsync(today.Month, today.Day, ct);

            foreach (var user in users)
            {
                try
                {
                    emailService.SendBirthdayGreeting(user, ct);
                }
                catch (Exception){}
            }
        }
    }
}