using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.Game;
using VideoGames.BLL.Dtos.GameDto;
using VideoGames.BLL.Dtos.Pagination;
using VideoGames.BLL.Services;
using VideoGames.BLL.Tools;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class GameService
    {
        private readonly DeveloperRepository _developerRepository;
        private readonly GameRepository _gameRepository;
        private readonly GenreRepository _genreRepository;
        private readonly FileService _fileService;
        private readonly PaginateCollection _paginateCollection;
        private readonly IMapper _mapper;

        public GameService(GameRepository gameRepository, IMapper mapper, FileService fileService, DeveloperRepository developerRepository, PaginateCollection paginateCollection, GenreRepository genreRepository)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
            _fileService = fileService;
            _developerRepository = developerRepository;
            _paginateCollection = paginateCollection;
            _genreRepository = genreRepository;
        }

        public async Task<ResponseDto> GetAllAsync(PaginationRequestDto dto, CancellationToken ct = default)
        {
            var query = _gameRepository.Games
                .Include(g => g.Developer)
                .Include(g => g.Genres)
                .OrderBy(g => g.Id);

            var result = await _paginateCollection.PaginateAsync(query, dto, ct);

            var dtos = _mapper.Map<List<GameDto>>(result.Items);

            var paginationResponseDto = new PaginationResponseDto<GameDto>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                PageCount = result.PageCount,
                Total = result.Total,
                Items = dtos
            };

            return ResponseDto.Success("Ігри отримано", paginationResponseDto);
        }

        public async Task<ResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _gameRepository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return ResponseDto.Error($"Гра з id '{id}' не знайдена");
            }

            var dto = _mapper.Map<GameDto>(entity);
            return ResponseDto.Success("Гру отримано", dto);
        }

        public async Task<ResponseDto> CreateAsync(CreateGameDto dto, string imagesFolder, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Game>(dto);

            if (dto.Image != null)
            {
                entity.Image = await _fileService.SaveImageAsync(dto.Image, imagesFolder, ct);
            }

            var genres = await _genreRepository.Genres
                .Where(g => dto.GenresId.Contains(g.Id))
                .ToListAsync(ct);

            entity.Genres = genres;

            await _gameRepository.CreateAsync(entity, ct);

            await _gameRepository.LoadDeveloperAsync(entity, ct);

            return ResponseDto.Success("Гру додано", _mapper.Map<GameDto>(entity));
        }

        public async Task<ResponseDto> DeleteAsync(int id, string imagesFolder, CancellationToken ct = default)
        {
            var entity = await _gameRepository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return ResponseDto.Error($"Гра з id '{id}' не знайдена");
            }

            if (!string.IsNullOrEmpty(entity.Image))
            {
                _fileService.DeleteFile(Path.Combine(imagesFolder, entity.Image));
            }

            await _gameRepository.DeleteAsync(id, ct);

            return ResponseDto.Success("Гру видалено");
        }

        public async Task<ResponseDto> UpdateAsync(UpdateGameDto dto, string imagesFolder, CancellationToken ct = default)
        {
            var entity = await _gameRepository.GetByIdAsync(dto.Id, ct);

            if (entity == null)
            {
                return ResponseDto.Error($"Гра з id '{dto.Id}' не знайдена");
            }

            var developer = await _developerRepository.GetAll()
                .FirstOrDefaultAsync(d => d.Id == dto.DeveloperId, ct);

            if (developer == null)
            {
                return ResponseDto.Error($"Розробник з id '{dto.DeveloperId}' не знайдений");
            }

            _mapper.Map(dto, entity);

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    _fileService.DeleteFile(Path.Combine(imagesFolder, entity.Image));
                }

                entity.Image = await _fileService.SaveImageAsync(dto.Image, imagesFolder, ct);
            }

            await _gameRepository.LoadGenresAsync(entity, ct);

            var genres = await _genreRepository.Genres
                .Where(g => dto.GenresId.Contains(g.Id))
                .ToListAsync(ct);

            entity.Genres = genres;

            await _gameRepository.UpdateAsync(entity, ct);

            return ResponseDto.Success("Дані про гру оновлено", _mapper.Map<GameDto>(entity));
        }

        public async Task<ResponseDto> AddGenreAsync(int gameId, string genreName, CancellationToken ct = default)
        {
            var game = await _gameRepository.GetByIdAsync(gameId, ct);

            if (game == null)
            {
                return ResponseDto.Error($"Гра з id '{gameId}' не знайдена");
            }

            await _gameRepository.LoadGenresAsync(game, ct);
            await _gameRepository.AddGenreAsync(game, genreName, ct);

            return ResponseDto.Success("Жанр додано до гри");
        }

        public async Task<ResponseDto> RemoveGenreAsync(int gameId, string genreName, CancellationToken ct = default)
        {
            var game = await _gameRepository.GetByIdAsync(gameId, ct);

            if (game == null)
            {
                return ResponseDto.Error($"Гра з id '{gameId}' не знайдена");
            }

            await _gameRepository.LoadGenresAsync(game, ct);
            await _gameRepository.RemoveGenreAsync(game, genreName, ct);

            return ResponseDto.Success("Жанр видалено з гри");
        }
    }
}