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

        [HttpGet("movie/{title}")]
        public async Task<IActionResult> SearchMovies(string title)
        {
            var results = await _tmdbApiClient.GetMovieByTitleAsync(title);
            return Ok(results);
        }

        [HttpGet("tvshow/{title}")]
        public async Task<IActionResult> SearchTvShows(string title)
        {
            var results = await _tmdbApiClient.GetTvShowByTitleAsync(title);
            return Ok(results);
        }
    }
}
