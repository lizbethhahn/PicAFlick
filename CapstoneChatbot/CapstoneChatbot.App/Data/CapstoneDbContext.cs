using CapstoneChatbot.App.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneChatbot.App.Data;

public class CapstoneDbContext : DbContext
{
    public CapstoneDbContext(DbContextOptions<CapstoneDbContext> options) : base(options) { }

    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WatchlistItem>(e =>
        {
            e.Property(x => x.Title).IsRequired().HasMaxLength(300);
            e.Property(x => x.MediaType)
             .HasConversion<string>()
             .IsRequired();
            e.Property(x => x.Overview).HasMaxLength(2000);
            e.Property(x => x.PosterPath).HasMaxLength(500);
            // Rating like 1.0, 1.5, 2.0 ... 5.0
            e.Property(x => x.Rating).HasPrecision(2, 1);
        });
    }
}
