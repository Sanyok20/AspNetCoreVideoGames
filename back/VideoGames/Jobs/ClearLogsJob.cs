using Quartz;

namespace VideoGames.Jobs
{
    public class ClearLogsJob : IJob
    {
        private readonly IWebHostEnvironment _env;

        public ClearLogsJob(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var root = _env.ContentRootPath;
            var logsPath = Path.Combine(root, "logs");
            var archivePath = Path.Combine(root, "archive");

            if (!Directory.Exists(archivePath))
            {
                Directory.CreateDirectory(archivePath);
            }

            var files = Directory.GetFiles(logsPath);

            foreach (var filePath in files)
            {
                var file = new FileInfo(filePath);
                if ((DateTime.UtcNow - file.CreationTimeUtc).Minutes > 7)
                {
                    file.Delete();
                }
            }

            return Task.CompletedTask;
        }
    }
}