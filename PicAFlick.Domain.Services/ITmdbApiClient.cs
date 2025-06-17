using Domain.DTOs;

namespace PicAFlick.Domain.Services
{
    public interface ITmdbApiClient
    {
        Task<TmdbMovieSearchResponse> GetMovieByTitleAsync(string query);
        Task<TmdbTvShowSearchResponse> GetTvShowByTitleAsync(string query);
    }
}