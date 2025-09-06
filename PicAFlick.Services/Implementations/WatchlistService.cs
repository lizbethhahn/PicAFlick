using PicAFlick.Data.Repositories;    
using PicAFlick.Domain.Entities;
using PicAFlick.Domain.Services.Mappers;
using PicAFlick.Services.Interfaces;
using PicAFlick.Shared.Contracts;

namespace PicAFlick.Services.Implementations
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _repo;
        //private readonly ITmdbApiClient _tmdb;

        public WatchlistService(IWatchlistRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WatchlistDisplayDto>> GetAllAsync(string userId, CancellationToken ct = default)
        {
            var entities = await _repo.GetAllAsync(userId, ct);
            return entities.Select(WatchlistMapper.MapToDisplayDto);
        }

        public async Task<WatchlistDisplayDto> GetByIdAsync(int id, string userId, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, userId, ct)
                         ?? throw new KeyNotFoundException($"No watchlist item found for Id {id} and User {userId}");
            return WatchlistMapper.MapToDisplayDto(entity);
        }

        public async Task<WatchlistDisplayDto?> AddAsync(WatchlistCreationDto dto, string userId, CancellationToken ct = default)
        {
            var media = await _repo.GetUserMediaByTmdbIdAsync(dto.TmdbId, ct);
            if (media is null)
            {
                media = await _repo.AddUserMediaAsync(new UserMedia
                {
                    TmdbId = dto.TmdbId,
                    Title = dto.Title,
                    MediaType = dto.MediaType,
                }, ct);
            }

            var entity = WatchlistMapper.MapFromCreationDto(dto, userId);
            entity.UserMediaId = media.Id;
            entity.UserMedia = null!;

            var savedEntity = await _repo.AddAsync(entity, ct);
            return WatchlistMapper.MapToDisplayDto(savedEntity);
        }

        public async Task RemoveEntryAsync(int id, string userId, CancellationToken ct = default)
            => await _repo.RemoveEntryAsync(id, userId, ct);

        public async Task MarkAsWatchedAsync(int id, string userId, CancellationToken ct = default)
            => await _repo.MarkAsWatchedAsync(id, userId, ct);
    }
}