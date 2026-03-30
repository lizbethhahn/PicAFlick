using Microsoft.AspNetCore.Mvc;
using PicAFlick.Infrastructure.Tmdb;
using PicAFlick.WebApi.Models;

namespace PicAFlick.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaChatController : ControllerBase
{
    private readonly ITmdbApiClient _tmdbApiClient;

    public MediaChatController(ITmdbApiClient tmdbApiClient)
    {
        _tmdbApiClient = tmdbApiClient;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Post([FromBody] MediaChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message is required.");
        }

        var message = request.Message.ToLower();

        if (message.Contains("who starred") && request.TmdbId.HasValue)
        {
            var creditsJson = await _tmdbApiClient.GetMovieCreditsAsync(request.TmdbId.Value);

            if (creditsJson == null)
            {
                return Ok("Could not retrieve cast right now.");
            }

            return Ok(creditsJson);
        }

        return Ok("I’m still learning, but I got your message!");
    }
}
