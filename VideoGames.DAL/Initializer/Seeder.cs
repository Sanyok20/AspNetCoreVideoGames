using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VideoGames.DAL.Entities;
using System;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL.Initializer
{
    public static class Seeder
    {
        public static async Task SeedAsync(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            if (await context.Developers.AnyAsync())
            {
                return;
            }

            List<Developer> developers = new()
            {
                new Developer
                {
                    Name = "CD Projekt Red",
                    Country = "Poland",
                    Year = 1994,
                    Description = "Polish video game developer known for story-driven RPG games and the Cyberpunk and The Witcher franchises.",
                    Games =
                    [
                        new Game
                        {
                            Name = "The Witcher",
                            ReleaseDate = new DateTime(2007, 10, 26).ToUniversalTime(),
                            Description = "Action RPG following the monster hunter Geralt of Rivia.",
                            Genre = "Action RPG",
                            Price = 9.99m,
                            Rating = 8.5f
                        },
                        new Game
                        {
                            Name = "The Witcher 2: Assassins of Kings",
                            ReleaseDate = new DateTime(2011, 5, 17).ToUniversalTime(),
                            Description = "Geralt becomes involved in political conflicts after the assassination of a king.",
                            Genre = "Action RPG",
                            Price = 19.99m,
                            Rating = 8.8f
                        },
                        new Game
                        {
                            Name = "The Witcher 3: Wild Hunt",
                            ReleaseDate = new DateTime(2015, 5, 19).ToUniversalTime(),
                            Description = "Geralt searches for Ciri while facing the supernatural Wild Hunt.",
                            Genre = "Open World RPG",
                            Price = 39.99m,
                            Rating = 9.7f
                        },
                        new Game
                        {
                            Name = "The Witcher 3: Hearts of Stone",
                            ReleaseDate = new DateTime(2015, 10, 13).ToUniversalTime(),
                            Description = "Expansion featuring Geralt's mysterious contract with Gaunter O'Dimm.",
                            Genre = "Action RPG",
                            Price = 9.99m,
                            Rating = 9.2f
                        },
                        new Game
                        {
                            Name = "The Witcher 3: Blood and Wine",
                            ReleaseDate = new DateTime(2016, 5, 31).ToUniversalTime(),
                            Description = "Geralt travels to Toussaint to investigate a series of mysterious murders.",
                            Genre = "Action RPG",
                            Price = 19.99m,
                            Rating = 9.6f
                        },
                        new Game
                        {
                            Name = "Cyberpunk 2077",
                            ReleaseDate = new DateTime(2020, 12, 10).ToUniversalTime(),
                            Description = "Open-world RPG set in the futuristic metropolis of Night City.",
                            Genre = "Action RPG",
                            Price = 59.99m,
                            Rating = 8.8f
                        },
                        new Game
                        {
                            Name = "Cyberpunk 2077: Phantom Liberty",
                            ReleaseDate = new DateTime(2023, 9, 26).ToUniversalTime(),
                            Description = "Spy-thriller expansion taking place in the dangerous district of Dogtown.",
                            Genre = "Action RPG",
                            Price = 29.99m,
                            Rating = 9.3f
                        },
                        new Game
                        {
                            Name = "Gwent: The Witcher Card Game",
                            ReleaseDate = new DateTime(2018, 10, 23).ToUniversalTime(),
                            Description = "Competitive digital card game based on the card game from The Witcher universe.",
                            Genre = "Card Game",
                            Price = 0.00m,
                            Rating = 8.0f
                        },
                        new Game
                        {
                            Name = "Thronebreaker: The Witcher Tales",
                            ReleaseDate = new DateTime(2018, 10, 23).ToUniversalTime(),
                            Description = "Story-driven RPG combining exploration and card-based combat.",
                            Genre = "RPG / Card Game",
                            Price = 19.99m,
                            Rating = 8.4f
                        },
                        new Game
                        {
                            Name = "The Witcher Adventure Game",
                            ReleaseDate = new DateTime(2014, 11, 27).ToUniversalTime(),
                            Description = "Digital adaptation of a board game set in The Witcher universe.",
                            Genre = "Strategy / Board Game",
                            Price = 9.99m,
                            Rating = 7.2f
                        }
                    ]
                },

                new Developer
                {
                    Name = "Rockstar Games",
                    Country = "United States",
                    Year = 1998,
                    Description = "Video game developer and publisher famous for large open-world games such as Grand Theft Auto and Red Dead Redemption.",
                    Games =
                    [
                        new Game
                        {
                            Name = "Grand Theft Auto III",
                            ReleaseDate = new DateTime(2001, 10, 23).ToUniversalTime(),
                            Description = "Open-world crime game set in Liberty City.",
                            Genre = "Action Adventure",
                            Price = 19.99m,
                            Rating = 9.1f
                        },
                        new Game
                        {
                            Name = "Grand Theft Auto: Vice City",
                            ReleaseDate = new DateTime(2002, 10, 29).ToUniversalTime(),
                            Description = "Crime-focused open-world game set in the 1980s-inspired Vice City.",
                            Genre = "Action Adventure",
                            Price = 19.99m,
                            Rating = 9.2f
                        },
                        new Game
                        {
                            Name = "Grand Theft Auto: San Andreas",
                            ReleaseDate = new DateTime(2004, 10, 26).ToUniversalTime(),
                            Description = "CJ returns to Los Santos and becomes involved in gangs and criminal organizations.",
                            Genre = "Action Adventure",
                            Price = 19.99m,
                            Rating = 9.5f
                        },
                        new Game
                        {
                            Name = "Bully",
                            ReleaseDate = new DateTime(2006, 10, 17).ToUniversalTime(),
                            Description = "Open-world action game set inside the fictional Bullworth Academy.",
                            Genre = "Action Adventure",
                            Price = 14.99m,
                            Rating = 8.7f
                        },
                        new Game
                        {
                            Name = "Grand Theft Auto IV",
                            ReleaseDate = new DateTime(2008, 4, 29).ToUniversalTime(),
                            Description = "Niko Bellic arrives in Liberty City searching for a new life.",
                            Genre = "Action Adventure",
                            Price = 19.99m,
                            Rating = 9.4f
                        },
                        new Game
                        {
                            Name = "Red Dead Redemption",
                            ReleaseDate = new DateTime(2010, 5, 18).ToUniversalTime(),
                            Description = "Western open-world adventure following former outlaw John Marston.",
                            Genre = "Action Adventure",
                            Price = 49.99m,
                            Rating = 9.4f
                        },
                        new Game
                        {
                            Name = "L.A. Noire",
                            ReleaseDate = new DateTime(2011, 5, 17).ToUniversalTime(),
                            Description = "Detective game set in post-war Los Angeles.",
                            Genre = "Detective / Adventure",
                            Price = 39.99m,
                            Rating = 8.6f
                        },
                        new Game
                        {
                            Name = "Max Payne 3",
                            ReleaseDate = new DateTime(2012, 5, 15).ToUniversalTime(),
                            Description = "Third-person shooter following Max Payne in São Paulo.",
                            Genre = "Third-Person Shooter",
                            Price = 19.99m,
                            Rating = 8.7f
                        },
                        new Game
                        {
                            Name = "Grand Theft Auto V",
                            ReleaseDate = new DateTime(2013, 9, 17).ToUniversalTime(),
                            Description = "Open-world crime game featuring Michael, Franklin and Trevor.",
                            Genre = "Action Adventure",
                            Price = 29.99m,
                            Rating = 9.6f
                        },
                        new Game
                        {
                            Name = "Red Dead Redemption 2",
                            ReleaseDate = new DateTime(2018, 10, 26).ToUniversalTime(),
                            Description = "Western adventure following Arthur Morgan and the Van der Linde gang.",
                            Genre = "Open World Action Adventure",
                            Price = 59.99m,
                            Rating = 9.8f
                        }
                    ]
                },

                new Developer
                {
                    Name = "Bethesda Game Studios",
                    Country = "United States",
                    Year = 2001,
                    Description = "American game developer best known for large-scale open-world RPG franchises including The Elder Scrolls and Fallout.",
                    Games =
                    [
                        new Game
                        {
                            Name = "The Elder Scrolls III: Morrowind",
                            ReleaseDate = new DateTime(2002, 5, 1).ToUniversalTime(),
                            Description = "Open-world fantasy RPG set on the island of Vvardenfell.",
                            Genre = "RPG",
                            Price = 14.99m,
                            Rating = 8.9f
                        },
                        new Game
                        {
                            Name = "The Elder Scrolls IV: Oblivion",
                            ReleaseDate = new DateTime(2006, 3, 20).ToUniversalTime(),
                            Description = "Fantasy RPG where the player attempts to stop a Daedric invasion.",
                            Genre = "Open World RPG",
                            Price = 14.99m,
                            Rating = 9.1f
                        },
                        new Game
                        {
                            Name = "Fallout 3",
                            ReleaseDate = new DateTime(2008, 10, 28).ToUniversalTime(),
                            Description = "Post-apocalyptic RPG set in the ruins of Washington, D.C.",
                            Genre = "Action RPG",
                            Price = 19.99m,
                            Rating = 9.1f
                        },
                        new Game
                        {
                            Name = "The Elder Scrolls V: Skyrim",
                            ReleaseDate = new DateTime(2011, 11, 11).ToUniversalTime(),
                            Description = "Open-world fantasy RPG following the Dragonborn.",
                            Genre = "Open World RPG",
                            Price = 39.99m,
                            Rating = 9.5f
                        },
                        new Game
                        {
                            Name = "Fallout Shelter",
                            ReleaseDate = new DateTime(2015, 6, 14).ToUniversalTime(),
                            Description = "Management simulation where players build and operate a Vault.",
                            Genre = "Simulation",
                            Price = 0.00m,
                            Rating = 7.8f
                        },
                        new Game
                        {
                            Name = "Fallout 4",
                            ReleaseDate = new DateTime(2015, 11, 10).ToUniversalTime(),
                            Description = "Open-world post-apocalyptic RPG set in the Commonwealth.",
                            Genre = "Action RPG",
                            Price = 29.99m,
                            Rating = 8.8f
                        },
                        new Game
                        {
                            Name = "Fallout 76",
                            ReleaseDate = new DateTime(2018, 11, 14).ToUniversalTime(),
                            Description = "Online multiplayer RPG set in post-apocalyptic West Virginia.",
                            Genre = "Online Action RPG",
                            Price = 39.99m,
                            Rating = 7.4f
                        },
                        new Game
                        {
                            Name = "The Elder Scrolls V: Skyrim Special Edition",
                            ReleaseDate = new DateTime(2016, 10, 28).ToUniversalTime(),
                            Description = "Remastered edition of Skyrim with improved graphics and mod support.",
                            Genre = "Open World RPG",
                            Price = 39.99m,
                            Rating = 9.2f
                        },
                        new Game
                        {
                            Name = "The Elder Scrolls: Blades",
                            ReleaseDate = new DateTime(2020, 5, 14).ToUniversalTime(),
                            Description = "Dungeon-focused RPG designed primarily for mobile platforms.",
                            Genre = "Action RPG",
                            Price = 0.00m,
                            Rating = 6.8f
                        },
                        new Game
                        {
                            Name = "Starfield",
                            ReleaseDate = new DateTime(2023, 9, 6).ToUniversalTime(),
                            Description = "Science-fiction RPG featuring planetary exploration and space travel.",
                            Genre = "Sci-Fi RPG",
                            Price = 69.99m,
                            Rating = 8.0f
                        }
                    ]
                },

                new Developer
                {
                    Name = "Ubisoft Montreal",
                    Country = "Canada",
                    Year = 1997,
                    Description = "Canadian development studio responsible for many major Ubisoft franchises including Assassin's Creed, Far Cry and Watch Dogs.",
                    Games =
                    [
                        new Game
                        {
                            Name = "Prince of Persia: The Sands of Time",
                            ReleaseDate = new DateTime(2003, 11, 6).ToUniversalTime(),
                            Description = "Action adventure featuring acrobatics and time manipulation.",
                            Genre = "Action Adventure",
                            Price = 9.99m,
                            Rating = 8.9f
                        },
                        new Game
                        {
                            Name = "Far Cry 2",
                            ReleaseDate = new DateTime(2008, 10, 21).ToUniversalTime(),
                            Description = "Open-world first-person shooter set in a fictional African country.",
                            Genre = "FPS",
                            Price = 9.99m,
                            Rating = 8.1f
                        },
                        new Game
                        {
                            Name = "Assassin's Creed II",
                            ReleaseDate = new DateTime(2009, 11, 17).ToUniversalTime(),
                            Description = "Ezio Auditore becomes an Assassin during the Italian Renaissance.",
                            Genre = "Action Adventure",
                            Price = 19.99m,
                            Rating = 9.2f
                        },
                        new Game
                        {
                            Name = "Assassin's Creed Brotherhood",
                            ReleaseDate = new DateTime(2010, 11, 16).ToUniversalTime(),
                            Description = "Ezio establishes an Assassin Brotherhood in Rome.",
                            Genre = "Action Adventure",
                            Price = 19.99m,
                            Rating = 8.9f
                        },
                        new Game
                        {
                            Name = "Far Cry 3",
                            ReleaseDate = new DateTime(2012, 11, 29).ToUniversalTime(),
                            Description = "Open-world shooter featuring Jason Brody and the pirate Vaas Montenegro.",
                            Genre = "Open World FPS",
                            Price = 19.99m,
                            Rating = 9.1f
                        },
                        new Game
                        {
                            Name = "Watch Dogs",
                            ReleaseDate = new DateTime(2014, 5, 27).ToUniversalTime(),
                            Description = "Open-world action game focused on hacking the infrastructure of Chicago.",
                            Genre = "Action Adventure",
                            Price = 29.99m,
                            Rating = 8.0f
                        },
                        new Game
                        {
                            Name = "Far Cry 4",
                            ReleaseDate = new DateTime(2014, 11, 18).ToUniversalTime(),
                            Description = "Open-world FPS set in the fictional Himalayan region of Kyrat.",
                            Genre = "Open World FPS",
                            Price = 29.99m,
                            Rating = 8.5f
                        },
                        new Game
                        {
                            Name = "Watch Dogs 2",
                            ReleaseDate = new DateTime(2016, 11, 15).ToUniversalTime(),
                            Description = "Hacktivist Marcus Holloway fights an advanced surveillance system in San Francisco.",
                            Genre = "Open World Action Adventure",
                            Price = 39.99m,
                            Rating = 8.4f
                        },
                        new Game
                        {
                            Name = "Far Cry 5",
                            ReleaseDate = new DateTime(2018, 3, 27).ToUniversalTime(),
                            Description = "Open-world shooter set in fictional Hope County, Montana.",
                            Genre = "Open World FPS",
                            Price = 39.99m,
                            Rating = 8.2f
                        },
                        new Game
                        {
                            Name = "Assassin's Creed Valhalla",
                            ReleaseDate = new DateTime(2020, 11, 10).ToUniversalTime(),
                            Description = "Viking-themed action RPG following the warrior Eivor.",
                            Genre = "Action RPG",
                            Price = 59.99m,
                            Rating = 8.3f
                        }
                    ]
                },

                new Developer
                {
                    Name = "FromSoftware",
                    Country = "Japan",
                    Year = 1986,
                    Description = "Japanese developer famous for challenging action RPG games including Dark Souls, Bloodborne, Sekiro and Elden Ring.",
                    Games =
                    [
                        new Game
                        {
                            Name = "Demon's Souls",
                            ReleaseDate = new DateTime(2009, 2, 5).ToUniversalTime(),
                            Description = "Dark fantasy action RPG known for difficult combat and atmospheric world design.",
                            Genre = "Action RPG",
                            Price = 39.99m,
                            Rating = 8.9f
                        },
                        new Game
                        {
                            Name = "Dark Souls",
                            ReleaseDate = new DateTime(2011, 9, 22).ToUniversalTime(),
                            Description = "Challenging dark fantasy RPG set in the kingdom of Lordran.",
                            Genre = "Action RPG",
                            Price = 39.99m,
                            Rating = 9.3f
                        },
                        new Game
                        {
                            Name = "Dark Souls II",
                            ReleaseDate = new DateTime(2014, 3, 11).ToUniversalTime(),
                            Description = "Action RPG where the player explores the kingdom of Drangleic.",
                            Genre = "Action RPG",
                            Price = 39.99m,
                            Rating = 8.6f
                        },
                        new Game
                        {
                            Name = "Bloodborne",
                            ReleaseDate = new DateTime(2015, 3, 24).ToUniversalTime(),
                            Description = "Gothic action RPG set in the cursed city of Yharnam.",
                            Genre = "Action RPG",
                            Price = 19.99m,
                            Rating = 9.4f
                        },
                        new Game
                        {
                            Name = "Dark Souls III",
                            ReleaseDate = new DateTime(2016, 3, 24).ToUniversalTime(),
                            Description = "Final main entry in the Dark Souls trilogy.",
                            Genre = "Action RPG",
                            Price = 59.99m,
                            Rating = 9.3f
                        },
                        new Game
                        {
                            Name = "Sekiro: Shadows Die Twice",
                            ReleaseDate = new DateTime(2019, 3, 22).ToUniversalTime(),
                            Description = "Action adventure following a shinobi searching for his kidnapped lord.",
                            Genre = "Action Adventure",
                            Price = 59.99m,
                            Rating = 9.2f
                        },
                        new Game
                        {
                            Name = "Elden Ring",
                            ReleaseDate = new DateTime(2022, 2, 25).ToUniversalTime(),
                            Description = "Open-world action RPG set in the Lands Between.",
                            Genre = "Open World Action RPG",
                            Price = 59.99m,
                            Rating = 9.7f
                        },
                        new Game
                        {
                            Name = "Armored Core VI: Fires of Rubicon",
                            ReleaseDate = new DateTime(2023, 8, 25).ToUniversalTime(),
                            Description = "Mecha combat game featuring extensive robot customization.",
                            Genre = "Mecha Action",
                            Price = 59.99m,
                            Rating = 8.9f
                        },
                        new Game
                        {
                            Name = "Elden Ring: Shadow of the Erdtree",
                            ReleaseDate = new DateTime(2024, 6, 21).ToUniversalTime(),
                            Description = "Large expansion for Elden Ring featuring the Land of Shadow.",
                            Genre = "Action RPG",
                            Price = 39.99m,
                            Rating = 9.5f
                        },
                        new Game
                        {
                            Name = "Dark Souls Remastered",
                            ReleaseDate = new DateTime(2018, 5, 25).ToUniversalTime(),
                            Description = "Remastered version of the original Dark Souls with improved visuals and performance.",
                            Genre = "Action RPG",
                            Price = 39.99m,
                            Rating = 9.0f
                        }
                    ]
                }
            };

            await context.Developers.AddRangeAsync(developers);
            await context.SaveChangesAsync();
        }
    }
}