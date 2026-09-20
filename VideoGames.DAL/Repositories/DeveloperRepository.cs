using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Repositories;

public class DeveloperRepository
{
    private readonly AppDbContext _context;

    public DeveloperRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Developer>> GetAll()
    {
        return await _context.Developers.ToListAsync();
    }

    public async Task<Developer?> GetById(int id)
    {
        return await _context.Developers.FindAsync(id);
    }

    public async Task Create(Developer developer)
    {
        await _context.Developers.AddAsync(developer);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Developer developer)
    {
        _context.Developers.Update(developer);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var developer = await _context.Developers.FindAsync(id);
        if (developer != null)
        {
            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();
        }
    }
}