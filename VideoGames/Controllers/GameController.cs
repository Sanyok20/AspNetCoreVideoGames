using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos.Game;
using VideoGames.BLL.Dtos.Pagination;
using VideoGames.BLL.Services;
using VideoGames.Extensions;
using VideoGames.Settings;

namespace SPR521_VideoGames.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly string _imagesFolder;
        private readonly IValidator<CreateGameDto> _valiatorCreate;
        private readonly IValidator<UpdateGameDto> _valiatorUpdate;

        public GameController(GameService gameService, IWebHostEnvironment webHostEnvironment, IValidator<CreateGameDto> valiator, IValidator<UpdateGameDto> valiatorUpdate)
        {
            _gameService = gameService;
            _valiatorCreate = valiator;
            _valiatorUpdate = valiatorUpdate;

            string root = webHostEnvironment.ContentRootPath;
            _imagesFolder = Path.Combine(root, FileSettings.Games);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationRequestDto dto, CancellationToken ct = default)
        {
            var response = await _gameService.GetAllAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken ct = default)
        {
            var response = await _gameService.GetByIdAsync(id, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateGameDto dto, CancellationToken ct = default)
        {
            var validationResult = await _valiatorCreate.ValidateAsync(dto, ct);

            if (!validationResult.IsValid)
            {
                return this.ValidationResponse(validationResult);
            }

            var response = await _gameService.CreateAsync(dto, _imagesFolder, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateGameDto dto, CancellationToken ct = default)
        {
            var validationResult = await _valiatorUpdate.ValidateAsync(dto, ct);

            if (!validationResult.IsValid)
            {
                return this.ValidationResponse(validationResult);
            }

            var response = await _gameService.UpdateAsync(dto, _imagesFolder, ct);
            return this.GetHttpResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken ct = default)
        {
            var response = await _gameService.DeleteAsync(id, _imagesFolder, ct);
            return this.GetHttpResponse(response);
        }
    }
}