using PicAFlick.Shared.Contracts;

namespace PicAFlick.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistDisplayDto?>> GetAllAsync(string? userId);
        Task<WatchlistDisplayDto?> GetByIdAsync(int id, string? userId);
        Task<WatchlistDisplayDto?> AddAsync(WatchlistCreationDto dto, string? userId);
        Task<bool> RemoveAsync(int id, string userId);
        Task RemoveEntryAsync(int id, string userId);
        Task MarkAsWatchedAsync(int id, string userId);   }
}