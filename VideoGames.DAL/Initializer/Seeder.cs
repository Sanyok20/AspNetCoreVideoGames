using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoGames.DAL;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Initializer
{
    public static class Seeder
    {
        public static async Task SeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

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
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();

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