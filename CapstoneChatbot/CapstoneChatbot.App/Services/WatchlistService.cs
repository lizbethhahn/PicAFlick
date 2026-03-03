using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneChatbot.App.Services;

public class WatchlistService
{
    private readonly CapstoneDbContext _db;

    public WatchlistService(CapstoneDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(string title, string mediaType, decimal? rating)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(mediaType))
            throw new ArgumentException("Media type is required.", nameof(mediaType));

        mediaType = mediaType.Trim().ToLowerInvariant();
        if (mediaType is not ("movie" or "tv"))
            throw new ArgumentException("Media type must be 'movie' or 'tv'.", nameof(mediaType));

        if (rating is < 1m or > 5m)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        if (rating is not null && (rating * 2) % 1 != 0)
            throw new ArgumentException("Rating must be in 0.5 increments (e.g., 3, 3.5, 4).", nameof(rating));

        var item = new WatchlistItem
        {
            Title = title.Trim(),
            MediaType = mediaType,
            Rating = rating,
            Watched = false
        };

        _db.WatchlistItems.Add(item);
        await _db.SaveChangesAsync();
    }

    public Task<List<WatchlistItem>> ListAsync()
        => _db.WatchlistItems
            .OrderByDescending(x => x.AddedAt)
            .ToListAsync();
}
