using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Repositories
{
    public interface IWatchlistRepository
    {
        Task<IEnumerable<WatchlistItem>> GetAllAsync(CancellationToken ct = default);
        Task<WatchlistItem> GetByIdAsync(int id, string? userId, CancellationToken ct = default);
        Task<WatchlistItem> AddAsync(WatchlistItem item, CancellationToken ct = default);
        Task RemoveFromWatchlistAsync(int id, CancellationToken ct = default);
        Task MarkAsWatchedAsync(int id, CancellationToken ct = default);
        Task<UserMedia?> GetUserMediaByTmdbIdAsync(int tmdbId, CancellationToken ct = default);
        Task<UserMedia> AddUserMediaAsync(UserMedia media, CancellationToken ct = default);
        Task<WatchlistItem?> GetByUserMediaIdAsync(int userMediaId, CancellationToken ct = default);
        Task UpdateUserMediaAsync(UserMedia media, CancellationToken ct = default);
    }    
}
