using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public required string NormalizedName { get; set; }

        public List<User> Users { get; set; } = [];
    }
}