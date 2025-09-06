using Microsoft.AspNetCore.Mvc;
using PicAFlick.Infrastructure.Tmdb;

namespace PicAFlick.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController(ITmdbApiClient tmdbApiClient) : ControllerBase
    {
        private readonly ITmdbApiClient _tmdbApiClient = tmdbApiClient;

        [HttpGet("movie/{query}")]
        public async Task<IActionResult> SearchMovies(string query)
        {
            var results = await _tmdbApiClient.GetMovieByTitleAsync(query);
            return Ok(results.Results);
        }

        [HttpGet("tv/{query}")]
        public async Task<IActionResult> SearchTvShows(string query)
        {
            var results = await _tmdbApiClient.GetTvShowByTitleAsync(query);
            return Ok(results.Results);
        }
    }
}
