using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.DAL.Initializer
{
    public static class Seeder
    {
        public static async Task SeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
            var roleRepository = scope.ServiceProvider.GetRequiredService<RoleRepository>();

            await context.Database.MigrateAsync();

            await SeedUsersRolesAsync(userRepository, roleRepository);
            await SeedDevelopersGamesAsync(context);
        }

        private static async Task SeedUsersRolesAsync(UserRepository userRepository, RoleRepository roleRepository)
        {
            if (!await roleRepository.Roles.AnyAsync())
            {
                var adminRole = new Role
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };

                await roleRepository.CreateAsync(adminRole);

                var userRole = new Role
                {
                    Name = "user",
                    NormalizedName = "USER"
                };

                await roleRepository.CreateAsync(userRole);
            }

            if (!await userRepository.Users.AnyAsync())
            {


                var admin = new User
                {
                    Email = "admin@mail.com",
                    NormalizedEmail = "ADMIN@MAIL.COM",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe"
                };

                await userRepository.CreateAsync(admin, "qwerty");

                var user = new User
                {
                    Email = "user@mail.com",
                    NormalizedEmail = "USER@MAIL.COM",
                    UserName = "user",
                    NormalizedUserName = "USER",
                    EmailConfirmed = true,
                    FirstName = "Mike",
                    LastName = "Thomson"
                };

                await userRepository.CreateAsync(user, "qwerty");

                await userRepository.AddToRoleAsync(admin, "admin");
                await userRepository.AddToRoleAsync(user, "user");
            }
        }

        private static async Task SeedDevelopersGamesAsync(AppDbContext context)
        {
            if (await context.Developers.AnyAsync())
            {
                return;
            }

            var root = Directory.GetCurrentDirectory();
            var developersPath = Path.Combine(root, "FileStorage", "seeder", "developers_with_genres.json");
            var genresPath = Path.Combine(root, "FileStorage", "seeder", "genres.json");

            if (!File.Exists(developersPath) || !File.Exists(genresPath))
            {
                throw new Exception("Seeder files not found. Please ensure that 'developers_with_genres.json' and 'genres.json' exist in the 'FileStorage/seeder' directory.");
            }

            var jsonGenres = await File.ReadAllTextAsync(genresPath);
            var jsonDevelopers = await File.ReadAllTextAsync(developersPath);

            var genres = JsonSerializer.Deserialize<List<Genre>>(jsonGenres);
            var developers = JsonSerializer.Deserialize<List<Developer>>(jsonDevelopers);

            if (developers != null && genres != null)
            {
                // genres
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();

                // developers
                var rnd = new Random();
                foreach (var dev in developers)
                {
                    foreach (var game in dev.Games)
                    {
                        var i1 = rnd.Next(0, genres.Count);
                        int i2 = 0;
                        do
                        {
                            i2 = rnd.Next(0, genres.Count);
                        }
                        while (i2 == i1);
                        game.Genres.AddRange(genres[i1], genres[i2]);
                    }
                }

                await context.Developers.AddRangeAsync(developers);
                await context.SaveChangesAsync();
            }
        }
    }
}