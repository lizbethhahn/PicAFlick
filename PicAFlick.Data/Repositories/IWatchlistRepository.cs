using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Repositories
{
    public interface IWatchlistRepository
    {
        Task<IEnumerable<WatchlistItem>> GetAllAsync(string? userId, CancellationToken ct = default);
        Task<WatchlistItem> GetByIdAsync(int id, string? userId, CancellationToken ct = default);
        Task<WatchlistItem> AddAsync(WatchlistItem item, CancellationToken ct = default);
        Task RemoveEntryAsync(int id, string userId, CancellationToken ct = default);
        Task MarkAsWatchedAsync(int id, string userId, CancellationToken ct = default);
        Task<UserMedia?> GetUserMediaByTmdbIdAsync(int tmdbId, CancellationToken ct = default);
        Task<UserMedia> AddUserMediaAsync(UserMedia media, CancellationToken ct = default);
    }    
}
