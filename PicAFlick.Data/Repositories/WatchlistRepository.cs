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

        public async Task<IEnumerable<WatchlistItem>> GetAllAsync()
        {
            return await _context.WatchlistItems.AsNoTracking().ToListAsync();
        }

        public async Task<WatchlistItem?> GetByIdAsync(int id)
        {
            return await _context.WatchlistItems.FindAsync(id);
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem item)
        {
            var entry = await _context.WatchlistItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _context.WatchlistItems.FindAsync(id);
            if (entity == null) return false;

            _context.WatchlistItems.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}