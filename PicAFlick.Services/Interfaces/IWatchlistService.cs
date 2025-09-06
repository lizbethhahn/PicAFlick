using PicAFlick.Shared.Contracts;

namespace PicAFlick.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistDisplayDto>> GetAllAsync(string userId, CancellationToken ct = default);
        Task<WatchlistDisplayDto> GetByIdAsync(int id, string userId, CancellationToken ct = default);
        Task<WatchlistDisplayDto?> AddAsync(WatchlistCreationDto dto, string userId, CancellationToken ct = default);
        Task RemoveEntryAsync(int id, string userId, CancellationToken ct = default);
        Task MarkAsWatchedAsync(int id, string userId, CancellationToken ct = default);
    }
}