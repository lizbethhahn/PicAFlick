using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Models;
using Microsoft.EntityFrameworkCore;
using CapstoneChatbot.Tmdb.Enums;
using CapstoneChatbot.App.Migrations;

namespace CapstoneChatbot.App.Services;

public class WatchlistService
{
    private readonly CapstoneDbContext _db;

    public WatchlistService(CapstoneDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(string title, DateTime? releaseDate, MediaType mediaType, int? tmdbId, decimal? rating)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (!Enum.IsDefined(typeof(MediaType), mediaType))
            throw new ArgumentException("Invalid media type.", nameof(mediaType));

        if (rating is < 1m or > 5m)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        if (rating is not null && (rating * 2) % 1 != 0)
            throw new ArgumentException("Rating must be in 0.5 increments (e.g., 3, 3.5, 4).", nameof(rating));

        var item = new WatchlistItem
        {
            Title = title.Trim(),
            ReleaseDate = releaseDate,
            MediaType = mediaType,
            TmdbId = tmdbId,
            Rating = rating,
            Watched = false
        };

        _db.WatchlistItems.Add(item);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        var item = await _db.WatchlistItems.FindAsync(id);

        if (item is null)
            throw new InvalidOperationException("Item not found.");

        _db.WatchlistItems.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task MarkWatchedAsync(int id)
    {
        var item = await _db.WatchlistItems.FindAsync(id);

        if (item is null)
            throw new InvalidOperationException("Item not found.");

        item.Watched = true;

        await _db.SaveChangesAsync();
    }

    public Task<List<WatchlistItem>> ListAsync()
        => _db.WatchlistItems
            .OrderByDescending(x => x.AddedAt)
            .ToListAsync();
}
