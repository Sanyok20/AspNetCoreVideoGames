using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;
using System;

namespace VideoGames.DAL.Repositories
{
    public class GameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Game> GetAll()
        {
            return _context.Games.AsNoTracking();
        }

        public async Task<Game?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == id, ct);
        }

        public async Task CreateAsync(Game entity, CancellationToken ct = default)
        {
            await _context.Games.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task CreateRangeAsync(IEnumerable<Game> entities, CancellationToken ct = default)
        {
            await _context.Games.AddRangeAsync(entities, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Game entity, CancellationToken ct = default)
        {
            _context.Games.Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Game entity, CancellationToken ct = default)
        {
            _context.Games.Remove(entity);
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