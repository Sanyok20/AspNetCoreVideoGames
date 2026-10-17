using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Repositories
{
    public class UserTokenRepository : GenericRepository<UserToken>
    {
        public UserTokenRepository(AppDbContext context)
            : base(context)
        {

        }
    }
}