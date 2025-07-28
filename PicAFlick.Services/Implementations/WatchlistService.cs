
using PicAFlick.Domain.Entities;
using PicAFlick.Services.Interfaces;
using PicAFlick.Data.Repositories;    
using PicAFlick.Infrastructure.Tmdb;  

namespace PicAFlick.Services.Implementations
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _repo;
        private readonly ITmdbApiClient _tmdb;

        public WatchlistService(
            IWatchlistRepository repo,
            ITmdbApiClient tmdb)
        {
            _repo = repo;
            _tmdb = tmdb;
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem item)
        {
            // Optionally enrich entity via TMDb:
            // var details = await _tmdb.GetMovieDetailsAsync(item.TmdbId);
            // item.Title = details.Title;
            // …etc.

            return await _repo.AddAsync(item);
        }

        public Task<IEnumerable<WatchlistItem>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<WatchlistItem?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public Task<bool> RemoveAsync(int id)
            => _repo.RemoveAsync(id);
    }
}



//using PicAFlick.Services.Interfaces;
//using PicAFlick.Data.Repositories;
//using PicAFlick.Infrastructure.Tmdb;
//using PicAFlick.Services.Interfaces;


//using AutoMapper;

//namespace PicAFlick.Services.Implementations
//{
//    public class WatchlistService : IWatchlistService
//    {
//        private readonly IWatchlistRepository _repo;
//        private readonly ITmdbApiClient _tmdb;
//        private readonly IMapper _mapper;

//        public WatchlistService(
//            IWatchlistRepository repo,
//            ITmdbApiClient tmdb,
//            IMapper mapper)
//        {
//            _repo = repo;
//            _tmdb = tmdb;
//            _mapper = mapper;
//        }

//        public async Task<WatchlistDisplayDto> AddEntryAsync(WatchlistCreationDto dto, string userId)
//        {
//            // 1) Map incoming DTO → entity
//            var entity = _mapper.Map<WatchlistItem>(dto);
//            entity.UserId = userId;

//            // optionally: fetch TMDb metadata here and populate additional fields

//            // 2) Persist
//            var saved = await _repo.AddAsync(entity);

//            // 3) Map persisted entity → display DTO
//            return _mapper.Map<WatchlistDisplayDto>(saved);
//        }

//        public async Task<IEnumerable<WatchlistDisplayDto>> GetEntriesAsync(string userId)
//        {
//            var entities = await _repo.GetByUserAsync(userId);
//            return _mapper.Map<IEnumerable<WatchlistDisplayDto>>(entities);
//        }

//        public async Task<WatchlistDisplayDto?> GetEntryAsync(int id, string userId)
//        {
//            var e = await _repo.GetByIdAsync(id, userId);
//            return e is null ? null : _mapper.Map<WatchlistDisplayDto>(e);
//        }

//        public Task<bool> RemoveEntryAsync(int id, string userId)
//            => _repo.RemoveAsync(id, userId);
//    }
//}


