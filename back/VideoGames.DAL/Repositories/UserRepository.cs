using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly AppDbContext _context;
        private readonly RoleRepository _roleRepository;

        public UserRepository(AppDbContext context, PasswordHasher<User> passwordHasher, RoleRepository roleRepository)
            : base(context)
        {
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _context = context;
        }

        public IQueryable<User> Users =>
            GetAll()
            .Include(u => u.Roles)
            .Include(u => u.Tokens);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper(), ct);
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken ct = default)
        {
            return await Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper(), ct);
        }

        public override async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<bool> IsExistsEmailAsync(string email, CancellationToken ct = default)
        {
            return await Users.AnyAsync(u => u.NormalizedEmail == email.ToUpper(), ct);
        }

        public async Task<bool> IsExistsUserNameAsync(string userName, CancellationToken ct = default)
        {
            return await Users.AnyAsync(u => u.NormalizedUserName == userName.ToUpper(), ct);
        }

        public async Task CreateAsync(User user, string password, CancellationToken ct = default)
        {
            string passwordHash = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;
            await CreateAsync(user, ct);
        }

        public bool CheckPassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken ct = default)
        {
            await _context.Entry(user).Collection(u => u.Roles).LoadAsync(ct);

            return user.Roles.Any(r => r.NormalizedName == roleName.ToUpper());
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken ct = default)
        {
            if (!await IsInRoleAsync(user, roleName, ct))
            {
                var role = await _roleRepository.GetByNameAsync(roleName, ct);

                if (role != null)
                {
                    user.Roles.Add(role);
                    await _context.SaveChangesAsync(ct);
                }
            }
        }

        public async Task DeleteFromRoleAsync(User user, string roleName, CancellationToken ct = default)
        {
            if (await IsInRoleAsync(user, roleName, ct))
            {
                user.Roles = user.Roles.Where(r => r.NormalizedName != roleName.ToUpper()).ToList();
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}