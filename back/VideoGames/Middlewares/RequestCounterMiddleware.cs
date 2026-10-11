namespace VideoGames.Middlewares
{
    public class RequestCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _requestCount = 0;

        public RequestCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            int currentCount = Interlocked.Increment(ref _requestCount);
            Console.WriteLine($"Request #{currentCount}: {context.Request.Method} {context.Request.Path}");

            await _next(context);
        }
    }
}