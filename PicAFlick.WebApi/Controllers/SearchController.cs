using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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

        [HttpGet("movie/{original_title}")]
        public async Task<IActionResult> SearchMovies(string original_title)
        {
            var results = await _tmdbApiClient.GetMovieByTitleAsync(original_title);
            return Ok(results.Results);
        }

        [HttpGet("tvShow/{original_name}")]
        public async Task<IActionResult> SearchTvShows(string original_name)
        {
            var results = await _tmdbApiClient.GetTvShowByTitleAsync(original_name);
            return Ok(results.Results);
        }
    }
}
