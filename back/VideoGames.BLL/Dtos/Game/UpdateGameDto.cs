using Microsoft.AspNetCore.Http;

namespace VideoGames.BLL.Dtos.Game
{
    public class UpdateGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }
        public int DeveloperId { get; set; }
        public List<int> GenresId { get; set; } = [];
    }
}