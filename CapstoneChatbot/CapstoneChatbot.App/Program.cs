using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Commands;
using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Helpers;
using CapstoneChatbot.App.Services;
using CapstoneChatbot.Tmdb.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

var tmdbApiClient = new TmdbApiClient(httpClient);

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

Console.WriteLine();
Console.WriteLine(ConsoleHelper.CommandPrompt);

while (true)
{
    Console.WriteLine();
    var cmd = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (cmd is "exit" or "quit" or "q")
        break;

    if (cmd is "list")
    {
        await ListCommand.ExecuteAsync(picAFlickApiClient);
    }

    if (cmd is "search")
    {
        await SearchCommand.ExecuteAsync(
            tmdbApiClient, 
            picAFlickApiClient, 
            configuration);
    }

    if (cmd is "add")
    {
        Console.WriteLine();
        Console.WriteLine("Manual add is not supported. Use 'search' to add items.");
        Console.WriteLine();
        Console.WriteLine(ConsoleHelper.CommandPrompt);
        continue;
    }

    if (cmd is "remove")
    {
        await RemoveCommand.ExecuteAsync(picAFlickApiClient);
    }

    if (cmd is "watched")
    {
        await WatchedCommand.ExecuteAsync(picAFlickApiClient);
    }

    if (cmd is "analyze")
    {
        await AnalyzeCommand.ExecuteAsync(picAFlickApiClient, configuration);
    }

    if (cmd is "chat")
    {
        await ChatCommand.ExecuteAsync(picAFlickApiClient, tmdbApiClient, configuration);
    }
}
