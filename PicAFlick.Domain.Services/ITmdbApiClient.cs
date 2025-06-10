using Domain.DTOs;

namespace PicAFlick.Domain.Services
{
    public interface ITmdbApiClient
    {
        Task<TmdbMovieSearchResponse> GetMovieByTitleAsync(string query, int page = 1);
        Task<TmdbMovieSearchResponse> GetTvShowByTitleAsync(string query, int page = 1);
    }
}