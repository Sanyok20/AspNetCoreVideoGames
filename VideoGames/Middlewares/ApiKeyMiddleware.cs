using VideoGames.BLL.Dtos;

namespace VideoGames.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var expectedApiKey = configuration["ApiKeySettings:SecretKey"] ?? "my-secret-api-key";

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey) ||
                !string.Equals(extractedApiKey, expectedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var responseDto = ResponseDto.Error("Unauthorized: Invalid or missing X-API-KEY header.");
                await context.Response.WriteAsJsonAsync(responseDto);
                return;
            }

            await _next(context);
        }
    }
}