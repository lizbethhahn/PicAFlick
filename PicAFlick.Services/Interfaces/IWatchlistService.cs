using PicAFlick.Domain.Entities;

namespace PicAFlick.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistItem>> GetAllAsync();
        Task<WatchlistItem?> GetByIdAsync(int id);
        Task<WatchlistItem> AddAsync(WatchlistItem item);
        Task<bool> RemoveAsync(int id);
    }
}