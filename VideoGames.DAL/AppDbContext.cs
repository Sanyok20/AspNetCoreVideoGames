using Microsoft.EntityFrameworkCore;
using VideoGames.DAL.Entities;

namespace VideoGames.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Developer>(e =>
            {
                e.HasKey(d => d.Id);

                e.Property(d => d.Name)
                .HasMaxLength(255)
                .IsRequired();

                e.Property(d => d.Country)
                .HasMaxLength(100);

                e.Property(d => d.Description)
                .HasColumnType("text");

                e.Property(d => d.Image)
                .HasMaxLength(50);
            });

            builder.Entity<Game>(e =>
            {
                e.HasKey(g => g.Id);

                e.Property(g => g.Name)
                .HasMaxLength(255)
                .IsRequired();

                e.Property(g => g.Description)
                .HasColumnType("text");

                e.Property(g => g.Image)
                .HasMaxLength(50);

                e.Property(g => g.Genre)
                .HasMaxLength(255);
            });

            builder.Entity<Game>()
                .HasOne(g => g.Developer)
                .WithMany(d => d.Games)
                .HasForeignKey(g => g.DeveloperId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}