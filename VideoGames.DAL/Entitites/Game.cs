using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }

        public int DeveloperId { get; set; }
        public Developer? Developer { get; set; }
    }
}