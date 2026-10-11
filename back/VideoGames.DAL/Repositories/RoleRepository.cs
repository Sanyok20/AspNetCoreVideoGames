using VideoGames.DAL;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Role> Roles => GetAll();
    }
}