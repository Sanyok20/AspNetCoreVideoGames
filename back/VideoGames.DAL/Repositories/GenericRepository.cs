using Microsoft.EntityFrameworkCore;
using VideoGames.DAL;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Repositories
{
    public class GenericRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(g => g.Id == id, ct);
        }

        public async Task CreateAsync(TEntity entity, CancellationToken ct = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity != null)
            {
                await DeleteAsync(entity, ct);
            }
        }
    }
}