using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoGames.DAL;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public UserRepository(AppDbContext context, PasswordHasher<User> passwordHasher)
            : base(context)
        {
            _passwordHasher = passwordHasher;
        }

        public IQueryable<User> Users =>
            GetAll()
            .Include(u => u.Roles);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper(), ct);
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken ct = default)
        {
            return await Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper(), ct);
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
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}