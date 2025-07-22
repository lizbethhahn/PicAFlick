using PicAFlick.Domain.Core.Models;

namespace PicAFlick.Domain.Core.Interfaces
{
    public interface ITmdbApiClient
    {
        Task<TmdbMovieSearchResponseDto> GetMovieByTitleAsync(string query);
        Task<TmdbTvShowSearchResponseDto> GetTvShowByTitleAsync(string query);
    }
}