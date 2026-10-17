using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoGames.BLL.Dtos;
using VideoGames.BLL.Dtos.Role;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(RoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _roleRepository.Roles.AsNoTracking().ToListAsync(ct);

            var dtos = _mapper.Map<List<RoleDto>>(entities);

            return ResponseDto.Success("Ролі отримано", dtos);
        }

        public async Task<ResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return ResponseDto.Error($"Роль з id '{id}' не знайдена");
            }

            var dto = _mapper.Map<RoleDto>(entity);

            return ResponseDto.Success("Роль отримано", dto);
        }

        public async Task<ResponseDto> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return ResponseDto.Error($"Роль з id '{id}' не знайдена");
            }

            await _roleRepository.DeleteAsync(entity, ct);

            return ResponseDto.Success("Роль видалено");
        }

        public async Task<ResponseDto> CreateAsync(CreateRoleDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Role>(dto);

            await _roleRepository.CreateAsync(entity, ct);

            return ResponseDto.Success("Роль додано", _mapper.Map<RoleDto>(entity));
        }

        public async Task<ResponseDto> UpdateAsync(UpdateRoleDto dto, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(dto.Id, ct);

            if (entity == null)
            {
                return ResponseDto.Error($"Роль з id '{dto.Id}' не знайдена");
            }

            _mapper.Map(dto, entity);

            await _roleRepository.UpdateAsync(entity, ct);

            return ResponseDto.Success("Роль оновлено", _mapper.Map<RoleDto>(entity));
        }
    }
}