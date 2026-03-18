using PicAFlick.Data.Repositories;    
using PicAFlick.Domain.Entities;
using PicAFlick.Domain.Services.Mappers;
using PicAFlick.Infrastructure.Tmdb;
using PicAFlick.Services.Interfaces;
using PicAFlick.Shared.Contracts;

namespace PicAFlick.Services.Implementations
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _repo;
       
        public WatchlistService(IWatchlistRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WatchlistDisplayDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _repo.GetAllAsync(ct);
            return entities.Select(WatchlistMapper.MapToDisplayDto);
        }

        public async Task<WatchlistDisplayDto> GetByIdAsync(int id, string userId, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, userId, ct)
                         ?? throw new KeyNotFoundException($"No watchlist item found for Id {id} and User {userId}");
            return WatchlistMapper.MapToDisplayDto(entity);
        }

        public async Task<WatchlistDisplayDto?> AddAsync(WatchlistCreationDto dto, CancellationToken ct = default)
        {
            var media = await _repo.GetUserMediaByTmdbIdAsync(dto.TmdbId, ct);
            if (media is not null && media.ReleaseDate == null && dto.ReleaseDate.HasValue)
            {
                media.ReleaseDate = dto.ReleaseDate;
                await _repo.UpdateUserMediaAsync(media, ct);
            }

            var existingItem = await _repo.GetByUserMediaIdAsync(media.Id, ct);
            if (existingItem is not null)
            {
                return WatchlistMapper.MapToDisplayDto(existingItem);
            }

            var entity = WatchlistMapper.MapFromCreationDto(dto);
            entity.UserMediaId = media.Id;
            entity.UserMedia = null!;

            var savedEntity = await _repo.AddAsync(entity, ct);
            savedEntity.UserMedia = media;

            return WatchlistMapper.MapToDisplayDto(savedEntity);
        }

        public async Task RemoveFromWatchlistAsync(int id, CancellationToken ct = default)
            => await _repo.RemoveFromWatchlistAsync(id, ct);

        public async Task MarkAsWatchedAsync(int id, CancellationToken ct = default)
            => await _repo.MarkAsWatchedAsync(id, ct);
    }
}