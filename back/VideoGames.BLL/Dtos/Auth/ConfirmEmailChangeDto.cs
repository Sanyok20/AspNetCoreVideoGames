namespace VideoGames.BLL.Dtos.Auth
{
    public class ConfirmEmailChangeDto
    {
        public string Token { get; set; } = null!;
        public string NewEmail { get; set; } = null!;
    }
}