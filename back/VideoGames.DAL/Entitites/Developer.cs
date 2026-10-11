using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Entities
{
    public class Developer : BaseEntity
    {
        public required string Name { get; set; }
        public string? Country { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        public List<Game> Games { get; set; } = [];
    }
}