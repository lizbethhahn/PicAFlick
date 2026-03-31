using PicAFlick.Infrastructure.Tmdb.Models;
using PicTmdb.Models;

namespace PicAFlick.Infrastructure.Tmdb
{
    public interface ITmdbApiClient
    {
        Task<TmdbMovieSearchResponseDto?> GetMovieByTitleAsync(string query);
        Task<TmdbTvShowSearchResponseDto?> GetTvShowByTitleAsync(string query);
        Task<TmdbMovieCreditsResponseDto?> GetMovieCreditsAsync(int tmdbId);
        Task<TmdbMovieCreditsResponseDto?> GetTvCreditsAsync(int tmdbId);
    }
}