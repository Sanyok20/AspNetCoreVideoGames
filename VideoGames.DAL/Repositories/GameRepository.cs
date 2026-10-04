using VideoGames.DAL.Entities;
using VideoGames.DAL;
using VideoGames.DAL.Repositories;

namespace VideoGames.DAL.Repositories
{
    public class GameRepository : GenericRepository<Game>
    {
        private readonly AppDbContext _context;
        private readonly GenreRepository _genreRepository;

        public GameRepository(AppDbContext context, GenreRepository genreRepository)
            : base(context)
        {
            _context = context;
            _genreRepository = genreRepository;
        }

        public IQueryable<Game> Games => GetAll();

        public async Task LoadDeveloperAsync(Game game, CancellationToken ct = default)
        {
            await _context.Entry(game).Reference(g => g.Developer).LoadAsync(ct);
        }

        public async Task LoadGenresAsync(Game game, CancellationToken ct = default)
        {
            await _context.Entry(game).Collection(g => g.Genres).LoadAsync(ct);
        }

        public async Task AddGenreAsync(Game game, string genreName, CancellationToken ct = default)
        {
            if (!game.Genres.Any(g => g.Name.ToLower() == genreName.ToLower()))
            {
                var genre = await _genreRepository.GetByNameAsync(genreName, ct);
                if (genre != null)
                {
                    game.Genres.Add(genre);
                    await _context.SaveChangesAsync(ct);
                }
            }
        }

        public async Task RemoveGenreAsync(Game game, string genreName, CancellationToken ct = default)
        {
            game.Genres = game.Genres.Where(g => g.Name.ToLower() != genreName.ToLower()).ToList();
            await _context.SaveChangesAsync(ct);
        }
    }
}