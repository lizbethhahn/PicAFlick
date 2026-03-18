using Microsoft.EntityFrameworkCore;
using PicAFlick.Data.Context;
using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly WatchlistContext _context;

        public WatchlistRepository(WatchlistContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WatchlistItem>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.WatchlistItems
                                 .AsNoTracking()
                                 .Include(x => x.UserMedia)
                                 .ToListAsync(ct);
        }

        public async Task<WatchlistItem> GetByIdAsync(int id, string? userId, CancellationToken ct = default)
        {
            var entity = await _context.WatchlistItems
                                       .AsNoTracking()
                                       .Include(x => x.UserMedia)
                                       .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
            if (entity is null)
                throw new KeyNotFoundException($"No watchlist item {id} for user {userId}");

            return entity;
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem item, CancellationToken ct = default)
        {
            _context.WatchlistItems.Add(item);
            await _context.SaveChangesAsync(ct);
            return item;
        }

        public async Task RemoveFromWatchlistAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.WatchlistItems
                                       .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null)
                throw new KeyNotFoundException($"No watchlist item with id {id}");

            _context.WatchlistItems.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task MarkAsWatchedAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.WatchlistItems
                                       .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null)
                throw new KeyNotFoundException($"No watchlist item with id {id}");

            entity.Watched = true;
            await _context.SaveChangesAsync(ct);
        }

        public Task<UserMedia?> GetUserMediaByTmdbIdAsync(int tmdbId, CancellationToken ct = default)
        {
            return _context.Set<UserMedia>()
                           .AsNoTracking()
                           .FirstOrDefaultAsync(m => m.TmdbId == tmdbId, ct);
        }

        public async Task<UserMedia> AddUserMediaAsync(UserMedia media, CancellationToken ct = default)
        {
            _context.Set<UserMedia>().Add(media);
            await _context.SaveChangesAsync(ct);
            return media;
        }

        public async Task<WatchlistItem?> GetByUserMediaIdAsync(int userMediaId, CancellationToken ct = default)
        {
            return await _context.WatchlistItems
                .AsNoTracking()
                .Include(x => x.UserMedia)
                .FirstOrDefaultAsync(x => x.UserMediaId == userMediaId, ct);
        }
        public async Task UpdateUserMediaAsync(UserMedia media, CancellationToken ct = default)
        {
            _context.UserMedia.Update(media);
            await _context.SaveChangesAsync(ct);
        }
    }
}