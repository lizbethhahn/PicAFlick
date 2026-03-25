using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Helpers;
using CapstoneChatbot.Tmdb.Clients;
using CapstoneChatbot.Tmdb.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using PicAFlick.Shared.Contracts;

namespace CapstoneChatbot.App.Commands;

public static class SearchCommand
{
    public static async Task ExecuteAsync(
        ITmdbApiClient tmdbClient, 
        IPicAFlickApiClient picClient,
        IConfiguration configuration)
    {
        Console.Write("Search Title: ");
        var query = Console.ReadLine() ?? "";

        // AI-assisted query cleanup
        try
        {
            var githubToken = configuration["GithubModels:ApiKey"];
            if (!string.IsNullOrEmpty(githubToken))
            {
                var kernelBuilder = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(
                        modelId: "openai/gpt-4o",
                        apiKey: githubToken,
                        endpoint: new Uri("https://models.github.ai/inference")
                    );

                var kernel = kernelBuilder.Build();

                var cleanupPrompt = $@"
                You are helping clean up a movie or TV title search query.

                Return ONLY a single cleaned title query.
                Do not explain.
                Do not use quotes.
                Do not add labels.
                Do not add extra text.

                If the input already looks usable, return it unchanged.

                Input:
                {query}
                ";

                var resultContext = await kernel.InvokePromptAsync(cleanupPrompt);
                var cleanedQuery = resultContext.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(cleanedQuery))
                {
                    query = cleanedQuery;
                    Console.WriteLine($"Cleaned query: {query}");
                }
            }
        }
        catch (Exception)
        {
            // Fall back to original query
        }

        MediaType mediaType;

        while (true)
        {
            Console.Write("Media Type (movie/tv): ");
            var mediaTypeText = Console.ReadLine() ?? "";

            mediaType = mediaTypeText.ToLowerInvariant() switch
            {
                "movie" => MediaType.Movie,
                "tv" => MediaType.TvShow,
                _ => MediaType.Unknown
            };

            if (mediaType != MediaType.Unknown)
                break;

            Console.WriteLine("Invalid media type. Use 'movie' or 'tv'.");
        }

        try
        {
            var results = await tmdbClient.SearchAsync(query, mediaType);

            if (results.Count == 0)
            {
                Console.WriteLine("No results found.");
                Console.WriteLine();
                Console.WriteLine(ConsoleHelper.CommandPrompt);
                return;
            }

            Console.WriteLine("Search Results:");
            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];

                string releaseYear = "Unknown";

                if (!string.IsNullOrWhiteSpace(result.ReleaseDate) &&
                    DateTime.TryParse(result.ReleaseDate, out var parsedReleaseDate))
                {
                    releaseYear = parsedReleaseDate.Year.ToString();
                }

                string posterUrl = string.IsNullOrWhiteSpace(result.PosterPath)
                    ? "Poster: none"
                    : $"Poster: https://image.tmdb.org/t/p/w500{result.PosterPath}";

                Console.WriteLine($"{i + 1}. {result.Title} ({releaseYear}) | {result.MediaType}");
                Console.WriteLine($"   {posterUrl}");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Enter a number to select an item to add to the watch list, or press Enter to return to the command prompt: ");
            var selectionInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(selectionInput))
            {
                Console.WriteLine();
                Console.WriteLine(ConsoleHelper.CommandPrompt);
                return;
            }

            if (!int.TryParse(selectionInput, out int selection))
            {
                Console.WriteLine("Invalid selection.");
                Console.WriteLine();
                Console.WriteLine(ConsoleHelper.CommandPrompt);
                return;
            }

            if (selection < 1 || selection > results.Count)
            {
                Console.WriteLine("Selection out of range.");
                Console.WriteLine();
                Console.WriteLine(ConsoleHelper.CommandPrompt);
                return;
            }

            // User sees results numbered 1..N, but List indexing is 0..N-1
            var chosenResult = results[selection - 1];

            DateTime? releaseDate = null;

            if (!string.IsNullOrWhiteSpace(chosenResult.ReleaseDate) &&
                DateTime.TryParse(chosenResult.ReleaseDate, out var parsedDate))
            {
                releaseDate = parsedDate;
            }

            await picClient.AddToWatchlistAsync(new WatchlistCreationDto
            {
                Title = chosenResult.Title,
                MediaType = (PicAFlick.Domain.Enums.MediaType)chosenResult.MediaType,
                TmdbId = chosenResult.TmdbId,
                ReleaseDate = releaseDate
            });

            Console.WriteLine();
            Console.WriteLine($"{chosenResult.Title} was added.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during search: {ex.Message}");
        }
        return;
    }
}