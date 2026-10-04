namespace VideoGames.BLL.Dtos.GameDto
{
    public class GameDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public float Rating { get; set; }
        public string? Developer { get; set; }
        public List<string> Genres { get; set; } = [];
    }
}