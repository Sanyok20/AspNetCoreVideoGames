using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Role> Roles => GetAll();

        public async Task<bool> IsExistsAsync(string name, CancellationToken ct = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .AnyAsync(r => r.NormalizedName == name.ToUpper(), ct);
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.NormalizedName == name.ToUpper(), ct);
        }
    }
}