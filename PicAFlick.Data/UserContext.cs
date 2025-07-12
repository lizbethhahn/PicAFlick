using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PicAFlick.Domain.Entities;

namespace PicAFlick.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<UserMedia> UserMedia { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ------------------- UserMedia -----------------------
            builder.Entity<UserMedia>(m =>
            {
                m.Property(p => p.UserRating)
                 .HasPrecision(3, 1);
            });

            // ------------------- WatchlistItem -------------------
            builder.Entity<WatchlistItem>(w =>
            {   
                w.HasOne(p => p.UserMedia)
                 .WithMany()
                 .HasForeignKey(p => p.UserMediaId)
                 .OnDelete(DeleteBehavior.Cascade);

                // creates composite index
                w.HasIndex(p => new { p.UserId, p.TmdbId }) 
                 .IsUnique(); 

                w.Property(p => p.Rating)
                 .HasColumnType("tinyint");
            });
        }
    }
}