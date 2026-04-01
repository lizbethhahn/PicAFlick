using Microsoft.AspNetCore.Mvc;
using PicAFlick.Infrastructure.Tmdb;
using PicAFlick.WebApi.Models;
using PicTmdb.Models;
using Microsoft.SemanticKernel;

namespace PicAFlick.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaChatController : ControllerBase
{
    private readonly ITmdbApiClient _tmdbApiClient;
    private readonly IConfiguration _configuration;
    public MediaChatController(ITmdbApiClient tmdbApiClient, IConfiguration configuration)
    {
        _tmdbApiClient = tmdbApiClient;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Post([FromBody] MediaChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message is required.");
        }

        var githubToken = _configuration["GithubModels:ApiKey"];
    
        if (!string.IsNullOrEmpty(githubToken))
        {
            var kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: "openai/gpt-4o-mini",
                    apiKey: githubToken,
                    endpoint: new Uri("https://models.github.ai/inference")
                )
                .Build();

            var intentPrompt = $@"
                You are classifying a user message.

                If the user is asking about who starred in a movie or show, respond with:
                CAST

                Otherwise respond with:
                OTHER

                Message:
                {request.Message}
                ";

            var intentResult = await kernel.InvokePromptAsync(intentPrompt);
            var intent = intentResult.ToString().Trim();

            if (intent.Contains("CAST") && request.TmdbId.HasValue)
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
        }

        return Ok("I’m still learning, but I got your message!");
    }
    private static string FormatTopCast(TmdbMovieCreditsResponseDto credits)
    {
        var topCast = credits.Cast
            .Take(5)
            .Select(c => $"* {c.Name} as {c.Character}");

        return "Here’s the top cast:\n\n" + string.Join("\n", topCast);
    }
}
