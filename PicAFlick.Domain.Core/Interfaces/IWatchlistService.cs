using PicAFlick.Domain.Core.Models;

namespace PicAFlick.Domain.Core.Interfaces
{
    public interface IWatchlistService
    {
        // Create
        Task<WatchlistDisplayDto> AddItemAsync(WatchlistCreationDto dto);

        // Read
        Task<List<WatchlistDisplayDto>> GetItemsByUserAsync(string userId);
        Task<WatchlistDisplayDto?> GetItemByIdAsync(int id, string userId);

        // Delete
        Task<bool> RemoveItemAsync(int id, string userId);

    }
}
