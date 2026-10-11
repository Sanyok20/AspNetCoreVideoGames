using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos.Genre;
using VideoGames.BLL.Services;
using VideoGames.Extensions;

namespace VideoGames.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;
        private readonly ILogger<GenreController> _logger;
        private readonly IValidator<CreateGenreDto> _createGenreValidator;
        private readonly IValidator<UpdateGenreDto> _updateGenreValidator;

        public GenreController(GenreService genreService, IValidator<CreateGenreDto> createGenreValidator, IValidator<UpdateGenreDto> updateGenreValidator, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _createGenreValidator = createGenreValidator;
            _updateGenreValidator = updateGenreValidator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogInformation(12, $"[{DateTime.Now}] Information: Getting all genres.");
            var response = await _genreService.GetAllAsync(ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken ct = default)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"[{DateTime.Now}] Information: Getting genre by ID '{id}' from IP '{ip}'.");

            var response = await _genreService.GetByIdAsync(id, ct);
            return this.GetHttpResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken ct = default)
        {
            var response = await _genreService.DeleteAsync(id, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateGenreDto dto, CancellationToken ct = default)
        {
            var validation = await _createGenreValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var response = await _genreService.CreateAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGenreDto dto, CancellationToken ct = default)
        {
            var validation = await _updateGenreValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var response = await _genreService.UpdateAsync(dto, ct);
            return this.GetHttpResponse(response);
        }
    }
}