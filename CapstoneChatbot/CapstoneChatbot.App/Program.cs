using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Services;
using CapstoneChatbot.App.Models;
using CapstoneChatbot.Tmdb.Enums;
using CapstoneChatbot.Tmdb.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Formats.Tar;
using PicAFlick.Shared.Contracts;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var tmdbApiToken = configuration["Tmdb:ApiToken"]
    ?? throw new InvalidOperationException("TMDb API token is missing.");

using var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.themoviedb.org/3/")
};

httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmdbApiToken}");

var tmdbClient = new TmdbClient(httpClient);

var dbPath = Path.Combine(AppContext.BaseDirectory, "capstone.db");
var connectionString = $"Data Source={dbPath}";

var options = new DbContextOptionsBuilder<CapstoneDbContext>()
    .UseSqlite(connectionString)
    .Options;

await using var db = new CapstoneDbContext(options);

var picHttpClient = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7043")
};

var picAFlickApiClient = new PicAFlickApiClient(picHttpClient);
var watchlist = new WatchlistService(db, tmdbClient);
var watchlistAnalyzer = new WatchlistAnalyzer();

var picWatchlist = await picAFlickApiClient.GetWatchlistAsync();
var analysisResult = watchlistAnalyzer.Analyze((IEnumerable<WatchlistItemDto>)picWatchlist);

Console.WriteLine($"Fetched {analysisResult.TotalCount} watchlist items.\n");

Console.WriteLine($"Movies: {analysisResult.MovieCount}");
Console.WriteLine($"TV Shows: {analysisResult.TvShowCount}");
Console.WriteLine($"Watched: {analysisResult.WatchedCount}");
Console.WriteLine($"Unwatched: {analysisResult.UnwatchedCount}\n");

Console.WriteLine("Here is your watchlist:");

foreach (var item in picWatchlist.OrderByDescending(x => x.Id).Take(5))
{
    var releaseYear = item.ReleaseDate.HasValue
        ? item.ReleaseDate.Value.Year.ToString()
        : "Unknown";
    Console.WriteLine($"{item.Title} - {releaseYear}  ({item.MediaType}) | Watched: {item.Watched} | TmdbId: {item.TmdbId}");
}

const string CommandPrompt = "Available watchlist commands:\n> Commands: add | list | search | remove | watched | exit";

Console.Write("\n> ");
Console.WriteLine(CommandPrompt);

while (true)
{
    Console.Write("\n> ");
    var cmd = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (cmd is "exit" or "quit" or "q")
        break;

    if (cmd is "list")
    {
        var items = await picAFlickApiClient.GetWatchlistAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Watchlist is empty.");
            continue;
        }


        foreach (var item in items)
        {
            var releaseYear = item.ReleaseDate.HasValue && item.ReleaseDate.Value.Year > 1
                ? item.ReleaseDate.Value.Year.ToString()
                : "Unknown";
            
            Console.WriteLine($"{item.Title} - {releaseYear}  ({item.MediaType}) | Watched: {item.Watched} | TmdbId: {item.TmdbId}");
        }

        Console.Write("\n> ");
        Console.WriteLine(CommandPrompt);
        continue;
    }

    if (cmd is "search")
    {
        Console.Write("Search Title: ");
        var query = Console.ReadLine() ?? "";

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
                Console.WriteLine(CommandPrompt);
                continue;
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

                Console.WriteLine($"{i + 1}. {result.Title} ({releaseYear}) | {result.MediaType}");
            }

            Console.WriteLine();
            Console.Write("Enter a number to select an item to add to the watch list, or press Enter to return to the command prompt: ");
            var selectionInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(selectionInput))
            {
                Console.WriteLine(CommandPrompt);
                continue;
            }

            if (!int.TryParse(selectionInput, out int selection))
            {
                Console.WriteLine("Invalid selection.");
                Console.WriteLine(CommandPrompt);
                continue;
            }

            if (selection < 1 || selection > results.Count)
            {
                Console.WriteLine("Selection out of range.");
                Console.WriteLine(CommandPrompt);
                continue;
            }

            // User sees results numbered 1..N, but List indexing is 0..N-1
            var chosenResult = results[selection - 1];

            DateTime? releaseDate = null;

            if (!string.IsNullOrWhiteSpace(chosenResult.ReleaseDate) &&
                DateTime.TryParse(chosenResult.ReleaseDate, out var parsedDate))
            {
                releaseDate = parsedDate;
            }

            await picAFlickApiClient.AddToWatchlistAsync(new WatchlistCreationDto
            {
                Title = chosenResult.Title,
                MediaType = (PicAFlick.Domain.Enums.MediaType)chosenResult.MediaType,
                TmdbId = chosenResult.TmdbId,
                ReleaseDate = releaseDate
            });

            Console.Write("\n> ");
            Console.WriteLine($"{chosenResult.Title} was added.");
            Console.Write("\n> ");
            Console.WriteLine(CommandPrompt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during search: {ex.Message}");
        }
        continue;
    }

    if (cmd is "add")
    {
        Console.Write("\n> ");
        Console.WriteLine("Manual add is not supported. Use 'search' to add items.");
        Console.WriteLine(CommandPrompt);
        continue;
    }

    if (cmd is "remove")
    {
        var items = await picAFlickApiClient.GetWatchlistAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Watchlist is empty.");
            Console.WriteLine(CommandPrompt);
            continue;
        }

        Console.WriteLine("Watchlist:");
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            var releaseYear = item.ReleaseDate.HasValue
                ? item.ReleaseDate.Value.Year.ToString()
                : "Unknown";

            Console.WriteLine($"{i + 1}. {item.Title} - {releaseYear} ({item.MediaType})");
        }

        Console.Write("Enter a number to remove an item, or press Enter to return to the command prompt: ");
        var removeInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(removeInput))
        {
            Console.WriteLine(CommandPrompt);
            continue;
        }

        if (!int.TryParse(removeInput, out int removeSelection))
        {
            Console.WriteLine("Invalid selection.");
            Console.WriteLine(CommandPrompt);
            continue;
        }

        if (removeSelection < 1 || removeSelection > items.Count)
        {
            Console.WriteLine("Selection out of range.");
            Console.WriteLine(CommandPrompt);
            continue;
        }

        var itemToRemove = items[removeSelection - 1];

        try
        {   
            await picAFlickApiClient.RemoveFromWatchlistAsync(itemToRemove.Id);
            Console.WriteLine("Item removed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not remove item: {ex.Message}");
        }

        Console.Write("\n> ");
        Console.WriteLine(CommandPrompt);
        continue;
    }

    if (cmd is "watched")
    {
        var items = await picAFlickApiClient.GetWatchlistAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Watchlist is empty.");
            Console.WriteLine(CommandPrompt);
            continue;
        }

        Console.WriteLine("Watchlist:");
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            var releaseYear = item.ReleaseDate.HasValue
                ? item.ReleaseDate.Value.Year.ToString()
                : "Unknown";

            Console.WriteLine($"{i + 1}. {item.Title} - {releaseYear} ({item.MediaType}) | Watched: {item.Watched}");
        }

        Console.Write("Enter a number to mark an item as watched, or press Enter to return to the command prompt: ");
        var watchedInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(watchedInput))
        {
            Console.WriteLine(CommandPrompt);
            continue;
        }

        if (!int.TryParse(watchedInput, out int watchedSelection))
        {
            Console.WriteLine("Invalid selection.");
            Console.WriteLine(CommandPrompt);
            continue;
        }

        if (watchedSelection < 1 || watchedSelection > items.Count)
        {
            Console.WriteLine("Selection out of range.");
            Console.WriteLine(CommandPrompt);
            continue;
        }

        var itemToMarkWatched = items[watchedSelection - 1];

        try
        {
            await picAFlickApiClient.MarkAsWatchedAsync(itemToMarkWatched.Id);
            Console.WriteLine("Item marked as watched.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not mark item as watched: {ex.Message}");
        }

        Console.Write("\n> ");
        Console.WriteLine(CommandPrompt);
        continue;
    }

    Console.WriteLine("Unknown command.");
    Console.WriteLine(CommandPrompt);
}
