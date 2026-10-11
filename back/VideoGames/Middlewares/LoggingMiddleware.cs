namespace VideoGames.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            string message = $"IsHttps: {request.IsHttps}\n" +
                $"Method: {request.Method}\n" +
                $"Url: {request.Scheme}://{request.Host}{request.Path}";

            _logger.LogInformation(message);
            await _next(context);

            var response = context.Response;
            message = $"StatusCode: {response.StatusCode}";
            _logger.LogInformation(message);
        }
    }
}