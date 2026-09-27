using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VideoGames.BLL.Dtos.Game
{
    public class CreateGameDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }
        public int DeveloperId { get; set; }
    }
}