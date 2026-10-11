using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Entities
{
    public class Genre : BaseEntity
    {
        public required string Name { get; set; }

        public List<Game> Games { get; set; } = [];
    }
}