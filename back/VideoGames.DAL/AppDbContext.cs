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
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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
            });

            builder.Entity<Genre>(e =>
            {
                e.HasKey(g => g.Id);

                e.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);
            });

            builder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);

                e.Property(u => u.UserName)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(u => u.NormalizedUserName)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(u => u.Email)
                    .HasMaxLength(255)
                    .IsRequired();

                e.Property(u => u.NormalizedEmail)
                    .HasMaxLength(255)
                    .IsRequired();

                e.Property(u => u.FirstName)
                    .HasMaxLength(100);

                e.Property(u => u.LastName)
                    .HasMaxLength(100);

                e.Property(u => u.PasswordHash)
                    .HasMaxLength(150);

                e.Property(u => u.Image)
                    .HasMaxLength(50);

                e.Property(u => u.Phone)
                    .HasMaxLength(15);
            });

            builder.Entity<Role>(e =>
            {
                e.HasKey(r => r.Id);

                e.Property(r => r.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(r => r.NormalizedName)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            builder.Entity<Game>()
                .HasOne(g => g.Developer)
                .WithMany(d => d.Games)
                .HasForeignKey(g => g.DeveloperId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<Game>()
                .HasMany(g => g.Genres)
                .WithMany(g => g.Games)
                .UsingEntity("GameGenres");

            builder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity("UserRoles");
        }
    }
}