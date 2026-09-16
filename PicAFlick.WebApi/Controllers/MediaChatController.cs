using Microsoft.AspNetCore.Mvc;
using PicAFlick.Chat;
using PicAFlick.Infrastructure.Tmdb;
using PicAFlick.WebApi.Models;
using PicTmdb.Models;

namespace PicAFlick.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaChatController : ControllerBase
{
    private readonly ITmdbApiClient _tmdbApiClient;
    private readonly ChatService _chatService;
    public MediaChatController(ITmdbApiClient tmdbApiClient, ChatService chatService)
    {
        _tmdbApiClient = tmdbApiClient;
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Post([FromBody] MediaChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message is required.");
        }

        var message = request.Message.ToLower();

        if (
            request.TmdbId.HasValue &&
            (
                message.Contains("who starred") ||
                message.Contains("who stars") ||
                message.Contains("cast")
            )
        )
        {
            var mediaType = request.MediaType?.ToLower();

            var credits = mediaType == "tv"
                ? await _tmdbApiClient.GetTvCreditsAsync(request.TmdbId.Value)
                : await _tmdbApiClient.GetMovieCreditsAsync(request.TmdbId.Value);

            if (credits?.Cast == null || !credits.Cast.Any())
            {
                return Ok("No cast information found.");
            }

            return Ok(FormatTopCast(credits));
        }

        var aiResponse = await _chatService.ProcessIncomingMessageAsync(request.Message);

        return Ok(aiResponse);
    }
    private static string FormatTopCast(TmdbMovieCreditsResponseDto credits)
    {
        var topCast = credits.Cast
            .Take(5)
            .Select(c => $"* {c.Name} as {c.Character}");

        return "Here’s the top cast:\n\n" + string.Join("\n", topCast);
    }
}