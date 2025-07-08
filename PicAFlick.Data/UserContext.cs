using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PicAFlick.Domain.Entities;
using System.Text.Json;

namespace PicAFlick.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<UserMovie> UserMovies { get; set; }
        public DbSet<UserTVShow> UserTVShows { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var intListComparer = new ValueComparer<List<int>>(
                (a, b) => a.SequenceEqual(b),
                l => l.Aggregate(0, (hash, v) => HashCode.Combine(hash, v.GetHashCode())),
                l => l.ToList());

            // <-------------------UserMovie------------------->
            builder.Entity<UserMovie>(m =>
            {
                m.HasIndex(x => x.TmdbMovieId).IsUnique();
                m.Property(x => x.UserRating).HasPrecision(3, 1);

                m.Property(u => u.GenreIds)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                       v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null)
                            ?? new List<int>())
                   .HasColumnType("nvarchar(max)")
                   .Metadata.SetValueComparer(intListComparer);
            });

            // -------------------UserTvShow-------------------
            builder.Entity<UserTVShow>(t =>
            {
                t.HasIndex(x => x.TmdbTvShowId).IsUnique();
                t.Property(x => x.UserRating).HasPrecision(3, 1);

                t.Property(u => u.GenreIds)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                       v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null)
                            ?? new List<int>())
                   .HasColumnType("nvarchar(max)")
                   .Metadata.SetValueComparer(intListComparer);
            });

            // -------------------WatchlistItem-------------------
            builder.Entity<WatchlistItem>(w =>
            {               
                w.HasIndex(p => new { p.UserId, p.TmdbId }).IsUnique(); // creates composit index
                w.Property(p => p.Rating).HasColumnType("tinyint");
            });
        }
    }
}