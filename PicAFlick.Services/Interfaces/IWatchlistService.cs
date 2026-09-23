using PicAFlick.Shared.Contracts;

namespace PicAFlick.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistDisplayDto>> GetAllAsync(CancellationToken ct = default);
        Task<WatchlistDisplayDto> GetByIdAsync(int id, string userId, CancellationToken ct = default);
        Task<WatchlistDisplayDto?> AddAsync(WatchlistCreationDto dto, CancellationToken ct = default);
        Task RemoveFromWatchlistAsync(int id,CancellationToken ct = default);
        Task SetWatchedAsync(int id, bool watched, CancellationToken ct = default);
    }
}