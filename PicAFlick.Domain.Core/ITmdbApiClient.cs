using PicAFlick.Infrastructure.Tmdb.Models;

namespace PicAFlick.Infrastructure.Tmdb
{
    public interface ITmdbApiClient
    {
        Task<TmdbMovieSearchResponseDto> GetMovieByTitleAsync(string query);
        Task<TmdbTvShowSearchResponseDto> GetTvShowByTitleAsync(string query);
    }
}