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
            e.Property(x => x.MediaType).IsRequired().HasMaxLength(20);

            // Rating like 1.0, 1.5, 2.0 ... 5.0
            e.Property(x => x.Rating).HasPrecision(2, 1);
        });
    }
}
