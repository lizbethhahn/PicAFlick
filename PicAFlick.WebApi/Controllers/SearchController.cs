using Microsoft.AspNetCore.Mvc;
using PicAFlick.Domain.Services;

namespace PicAFlick.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ITmdbApiClient _tmdbApiClient;

        public SearchController(ITmdbApiClient tmdbApiClient)
        {
            _tmdbApiClient = tmdbApiClient;
        }

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
