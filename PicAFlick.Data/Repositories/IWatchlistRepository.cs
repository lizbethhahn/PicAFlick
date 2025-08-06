using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Repositories
{
    public interface IWatchlistRepository
    {
        Task<IEnumerable<WatchlistItem>> GetAllAsync(string userId);
        Task<WatchlistItem> GetByIdAsync(int id, string userId);
        Task<WatchlistItem> AddAsync(WatchlistItem item);
        Task<bool> RemoveAsync(int id, string userId);
        Task RemoveEntryAsync(int id, string userId);
        Task MarkAsWatchedAsync(int id, string userId);
    }
}
