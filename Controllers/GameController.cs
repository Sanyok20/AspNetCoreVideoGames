using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly GameRepository _gameRepository;

        public GameController(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            int total = await _gameRepository.GetAll().CountAsync();
            int pages = (int)Math.Ceiling((double)total / pageSize);

            page = page < 1 || page > pages ? 1 : page;
            pageSize = pageSize < 1 ? 20 : pageSize;

            var games = await _gameRepository
                .GetAll()
                .Include(g => g.Developer)
                .OrderBy(g => g.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            var dtos = games.Select(g => new GameDto
            {
                Id = g.Id,
                Description = g.Description,
                Rating = g.Rating,
                Developer = g.Developer!.Name,
                Genre = g.Genre,
                Name = g.Name,
                Price = g.Price,
                ReleaseDate = g.ReleaseDate
            });

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken ct = default)
        {
            var game = await _gameRepository.GetByIdAsync(id, ct);

            if (game != null)
            {
                return Ok(game);
            }
            else
            {
                return NotFound($"Не вдалося знайти книгу з id '{id}'");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Game game, CancellationToken ct = default)
        {
            game.ReleaseDate = game.ReleaseDate.ToUniversalTime();
            await _gameRepository.CreateAsync(game, ct);

            return Ok("Гру додано");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Game game, CancellationToken ct = default)
        {
            game.ReleaseDate = game.ReleaseDate.ToUniversalTime();
            await _gameRepository.UpdateAsync(game, ct);

            return Ok("Гру додано");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] Game game, CancellationToken ct = default)
        {
            game.ReleaseDate = game.ReleaseDate.ToUniversalTime();
            await _gameRepository.DeleteAsync(game, ct);

            return Ok("Гру додано");
        }
    }
}