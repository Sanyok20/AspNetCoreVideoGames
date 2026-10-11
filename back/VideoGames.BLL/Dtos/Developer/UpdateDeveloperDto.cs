namespace VideoGames.BLL.Dtos.Developer
{
    public class UpdateDeveloperDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Country { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}