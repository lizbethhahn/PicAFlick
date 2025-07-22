using PicAFlick.Data.Entities;
using PicAFlick.Data.Context;
using PicAFlick.Domain.Core.Interfaces;
using PicAFlick.Domain.Core.Models;

namespace PicAFlick.Data.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly WatchlistContext _context;

        public WatchlistRepository(WatchlistContext context)
        {
            _context = context;
        }

        // Create
        public async Task<WatchlistDisplayDto> AddItemAsync(WatchlistCreationDto dto)
        {
            var item = new WatchlistItem
            {
                UserId = dto.UserId,
                Title = dto.Title,
                MediaType = dto.MediaType,
                TmdbId = dto.TmdbId,
                PosterPath = dto.PosterPath,
                ReleaseYear = dto.ReleaseYear,
                Overview = dto.Overview,
                Notes = dto.Notes
            };

            await _context.WatchlistItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return new WatchlistDisplayDto
            {
                Id = item.Id,
                UserId = item.UserId,
                Title = item.Title,
                MediaType = item.MediaType,
                TmdbId = item.TmdbId,
                PosterPath = item.PosterPath,
                ReleaseYear = item.ReleaseYear,
                Overview = item.Overview,
                Notes = item.Notes,
                Watched = item.Watched
            };
        }

        // Read
        public async Task<List<WatchlistDisplayDto>> GetItemsByUserAsync(string userId)
        {
            // TODO: Query WatchlistItems and project to DTOs
            throw new NotImplementedException();
        }
        public async Task<WatchlistDisplayDto?> GetItemByIdAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        // Delete
        public async Task<bool> RemoveItemAsync(int id, string userId)
        {
            // TODO: Find by id and remove
            throw new NotImplementedException();
        }
    }
}