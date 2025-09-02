using Microsoft.EntityFrameworkCore;
using PicAFlick.Data.Context;
using PicAFlick.Domain.Entities;

namespace PicAFlick.Data.Repositories
{
    public class WatchlistRepository(WatchlistContext context) : IWatchlistRepository
    {
        private readonly WatchlistContext _context = context;

        public async Task<IEnumerable<WatchlistItem>> GetAllAsync(string? userId)
        {
            return await _context.WatchlistItems
                                 .AsNoTracking()
                                 .Where (x => x.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<WatchlistItem> GetByIdAsync(int id, string? userId)
        {
            var entity = await _context.WatchlistItems
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x =>
                                    x.Id == id &&
                                    x.UserId == userId);
            return entity ?? throw new KeyNotFoundException($"No WatchlistItem {id} for user {userId}");                                    
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem item)
        {
            var entry = await _context.WatchlistItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<bool> RemoveAsync(int id, string userId)
        { 
            var entity = await _context.WatchlistItems
                                       .FirstOrDefaultAsync(x =>
                                           x.Id == id &&
                                           x.UserId == userId)
                        ?? throw new KeyNotFoundException($"No watchlist item {id} for user {userId}");

            _context.WatchlistItems.Remove(entity);
            var changes = await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveEntryAsync(int id, string userId)
        { 
            var entity = await _context.WatchlistItems
                                       .FirstOrDefaultAsync(x =>
                                           x.Id == id &&    
                                           x.UserId == userId);
            if (entity == null)
                throw new KeyNotFoundException($"No watchlist item {id} for user {userId}");

            _context.WatchlistItems.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsWatchedAsync(int id, string userId)
        { 
            var entity = await _context.WatchlistItems
                                       .FirstOrDefaultAsync(x =>
                                           x.Id == id &&
                                           x.UserId == userId);
            if (entity == null)
                throw new KeyNotFoundException($"No watchlist item {id} for user {userId}");

            entity.Watched = true;

            await _context.SaveChangesAsync();
        }

        public Task<UserMedia?> GetUserMediaByTmdbIdAsync(int tmdbId, CancellationToken ct = default) =>
        _context.Set<UserMedia>().AsNoTracking().FirstOrDefaultAsync(m => m.TmdbId == tmdbId, ct);

        public async Task<UserMedia> AddUserMediaAsync(UserMedia media, CancellationToken ct = default)
        {
            _context.Set<UserMedia>().Add(media);
            await _context.SaveChangesAsync(ct);
            return media;
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem item, CancellationToken ct = default)
        {
            _context.Set<WatchlistItem>().Add(item);
            await _context.SaveChangesAsync(ct);
            return item;
        }
    }
}