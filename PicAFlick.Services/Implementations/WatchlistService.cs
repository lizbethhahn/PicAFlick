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

        public async Task<IEnumerable<WatchlistDisplayDto?>> GetAllAsync(string? userId)
        { 
            var entities = await _repo.GetAllAsync(userId);
            return entities.Select(WatchlistMapper.MapToDisplayDto);
        }    

        public async Task<WatchlistDisplayDto?> GetByIdAsync(int id, string? userId)
        {
            var entity = await _repo.GetByIdAsync(id, userId)
                         ?? throw new KeyNotFoundException($"No watchlist item found for Id {id} and User {userId}");
            return WatchlistMapper.MapToDisplayDto(entity);
        }

        public async Task<WatchlistDisplayDto?> AddAsync(WatchlistCreationDto dto, string? userId)
        {
            var media = await _repo.GetUserMediaByTmdbIdAsync(dto.TmdbId);
            if (media is null)
            {
                media = await _repo.AddUserMediaAsync(new UserMedia
                {
                    TmdbId = dto.TmdbId,
                    Title = dto.Title,
                    MediaType = dto.MediaType,
                    Watched = false,
                    Notes = null,
                    UserRating = null
                });
            }

            // 2) map WatchlistItem and set required fields
            var entity = WatchlistMapper.MapFromCreationDto(dto, userId);
            entity.UserMediaId = media.Id;     // << critical for the FK
            entity.UserMedia = null!;        // ensure EF doesn't try to insert duplicate nav

            // 3) save
            var savedEntity = await _repo.AddAsync(entity);
            return WatchlistMapper.MapToDisplayDto(savedEntity);
        }
        public async Task<bool> RemoveAsync(int id, string userId)
            => await _repo.RemoveAsync(id, userId);

        public async Task RemoveEntryAsync(int id, string userId)
            => await _repo.RemoveEntryAsync(id, userId);

        public async Task MarkAsWatchedAsync(int id, string userId)
            => await _repo.MarkAsWatchedAsync(id, userId);
    }
}