using Microsoft.EntityFrameworkCore;
using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Context
{
    public class WatchlistContext : DbContext
    {
        public WatchlistContext(DbContextOptions<WatchlistContext> options) : base(options) { }

        public DbSet<WatchlistItem> WatchlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchlistItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.TmdbId }).IsUnique();
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Rating).HasColumnType("tinyint");
            });
        }
    }
}