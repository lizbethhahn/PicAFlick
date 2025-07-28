using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Repositories
{
    public interface IWatchlistRepository
    {
        Task<IEnumerable<WatchlistItem>> GetAllAsync();
        Task<WatchlistItem?> GetByIdAsync(int id);
        Task<WatchlistItem> AddAsync(WatchlistItem item);
        Task<bool> RemoveAsync(int id);

    }
}
