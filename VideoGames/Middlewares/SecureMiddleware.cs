namespace VideoGames.Middlewares
{
    public class SecureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public SecureMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _env = env;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var header = context.Request.Headers.FirstOrDefault(x => x.Key == "secretWord");
            var value = header.Value.FirstOrDefault();

            var root = _env.ContentRootPath;
            var filePath = Path.Combine(root, "FileStorage", "keys.txt");

            var keys = await File.ReadAllLinesAsync(filePath);

            if (string.IsNullOrEmpty(value) || !keys.Contains(value))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid secret word.");
                return;
            }

            await _next(context);
        }
    }
}