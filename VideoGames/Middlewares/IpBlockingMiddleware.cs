using VideoGames.BLL.Dtos;

namespace VideoGames.Middlewares
{
    public class IpBlockingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HashSet<string> _blockedIps = new()
        {
            "192.168.1.100",
            "10.0.0.1"
        };

        public IpBlockingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            if (remoteIp != null && _blockedIps.Contains(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                var responseDto = ResponseDto.Error("Your IP address is blocked.");
                await context.Response.WriteAsJsonAsync(responseDto);
                return;
            }

            await _next(context);
        }
    }
}