using Microsoft.EntityFrameworkCore;
using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Context
{
    public class WatchlistContext : DbContext
    {
        public WatchlistContext(DbContextOptions<WatchlistContext> options) : base(options) { }

        public DbSet<WatchlistItem> WatchlistItems { get; set; }
        public DbSet<UserMedia> UserMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchlistItem>(entity =>
            {
                entity.Property(x => x.UserId).IsRequired();

                entity.HasOne(x => x.UserMedia)
                      .WithMany()                         
                      .HasForeignKey(x => x.UserMediaId)
                      .IsRequired();                     
                entity.HasIndex(x => new { x.UserId, x.UserMediaId }).IsUnique(); 
             // entity.Property(e => e.Rating).HasColumnType("tinyint");
            });

            modelBuilder.Entity<UserMedia>(entity =>
            {
                entity.Property(x => x.TmdbId).IsRequired();
                entity.Property(x => x.Title).IsRequired();
                entity.Property(x => x.MediaType).IsRequired();
                entity.HasIndex(x => x.TmdbId).IsUnique();
            });
        }
    }
}