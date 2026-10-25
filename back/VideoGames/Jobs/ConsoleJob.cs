using Quartz;

namespace VideoGames.Jobs
{
    public class ConsoleJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var now = DateTime.Now;
            ; Console.WriteLine($"[{now.ToString("HH:mm:ss")}] Quartz work");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}