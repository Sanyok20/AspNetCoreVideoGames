using System.Diagnostics;

namespace VideoGames.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Request received at {Time}", DateTime.UtcNow);

            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation("Response sent at {Time}. Total time: {ElapsedMilliseconds} ms",
                DateTime.UtcNow, stopwatch.ElapsedMilliseconds);
        }
    }
}