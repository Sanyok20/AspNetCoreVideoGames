using System.Diagnostics;

namespace VideoGames.Middlewares
{
    public class RequestConsoleLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestConsoleLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            string method = context.Request.Method;
            string url = $"{context.Request.Path}{context.Request.QueryString}";
            int statusCode = context.Response.StatusCode;
            long elapsed = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"[LOG] Method: {method} | URL: {url} | Status: {statusCode} | Time: {elapsed}ms");
        }
    }
}