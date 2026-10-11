using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllRolesAsync(CancellationToken ct = default)
        {
            return await _roleRepository.Roles.ToListAsync(ct);
        }

        public async Task<Role?> GetRoleByIdAsync(int id, CancellationToken ct = default)
        {
            return await _roleRepository.GetByIdAsync(id, ct);
        }

        public async Task<bool> CreateRoleAsync(string name, CancellationToken ct = default)
        {
            string normalizedName = name.ToUpper();

            bool exists = await _roleRepository.Roles.AnyAsync(r => r.NormalizedName == normalizedName, ct);
            if (exists)
            {
                return false; 
            }

            var role = new Role
            {
                Name = name,
                NormalizedName = normalizedName
            };

            await _roleRepository.CreateAsync(role, ct);
            return true;
        }
    }
}