using VideoGames.BLL.Dtos;

namespace VideoGames.Middlewares
{
    public class HttpsCheckMiddlware
    {
        private readonly RequestDelegate _next;

        public HttpsCheckMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var responseDto = ResponseDto.Error("HTTPS is required for this endpoint.");
                await context.Response.WriteAsJsonAsync(responseDto);

                return;
            }
        }
    }
}