using VideoGames.DAL;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Repositories
{
    public class GameRepository : GenericRepository<Game>
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Game> Games => GetAll();

        public async Task LoadDeveloperAsync(Game game, CancellationToken ct = default)
        {
            await _context.Entry(game).Reference(g => g.Developer).LoadAsync(ct);
        }
    }
}