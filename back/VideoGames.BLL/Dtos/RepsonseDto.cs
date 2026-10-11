namespace VideoGames.BLL.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Payload { get; set; }

        public static ResponseDto Success(string message, object? payload = null)
        {
            return new ResponseDto
            {
                IsSuccess = true,
                Message = message,
                Payload = payload
            };
        }

        public static ResponseDto Error(string message, object? payload = null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = message,
                Payload = payload
            };
        }
    }
}