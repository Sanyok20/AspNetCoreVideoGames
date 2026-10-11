using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.Developer;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class DeveloperService
    {
        private readonly DeveloperRepository _developerRepository;
        private readonly IMapper _mapper;

        public DeveloperService(DeveloperRepository developerRepository, IMapper mapper)
        {
            _developerRepository = developerRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllAsync(CancellationToken ct = default)
        {
            var developers = await _developerRepository
                .GetAll()
                .ToListAsync(ct);

            var dtos = _mapper.Map<List<DeveloperDto>>(developers);

            return ResponseDto.Success("Розробників отримано", dtos);
        }

        public async Task<ResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var developer = await _developerRepository.GetByIdAsync(id, ct);

            if (developer == null)
            {
                return ResponseDto.Error($"Розробник з id '{id}' не знайдений");
            }

            var dto = _mapper.Map<DeveloperDto>(developer);

            return ResponseDto.Success("Розробника отримано", dto);
        }

        public async Task<ResponseDto> CreateAsync(CreateDeveloperDto dto, CancellationToken ct = default)
        {
            var developer = _mapper.Map<Developer>(dto);

            await _developerRepository.CreateAsync(developer, ct);

            return ResponseDto.Success(
                "Розробника додано",
                _mapper.Map<DeveloperDto>(developer));
        }

        public async Task<ResponseDto> UpdateAsync(UpdateDeveloperDto dto, CancellationToken ct = default)
        {
            var developer = await _developerRepository.GetByIdAsync(dto.Id, ct);

            if (developer == null)
            {
                return ResponseDto.Error($"Розробник з id '{dto.Id}' не знайдений");
            }

            _mapper.Map(dto, developer);

            await _developerRepository.UpdateAsync(developer, ct);

            return ResponseDto.Success(
                "Дані про розробника оновлено",
                _mapper.Map<DeveloperDto>(developer));
        }

        public async Task<ResponseDto> DeleteAsync(int id, CancellationToken ct = default)
        {
            var developer = await _developerRepository.GetByIdAsync(id, ct);

            if (developer == null)
            {
                return ResponseDto.Error($"Розробник з id '{id}' не знайдений");
            }

            await _developerRepository.DeleteAsync(id, ct);

            return ResponseDto.Success("Розробника видалено");
        }
    }
}