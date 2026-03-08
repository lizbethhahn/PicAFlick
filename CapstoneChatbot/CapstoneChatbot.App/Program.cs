using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Services;
using CapstoneChatbot.Tmdb.Enums;
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

var tmdbClient = new TmdbClient(httpClient);

var dbPath = Path.Combine(AppContext.BaseDirectory, "capstone.db");
var connectionString = $"Data Source={dbPath}";

var options = new DbContextOptionsBuilder<CapstoneDbContext>()
    .UseSqlite(connectionString)
    .Options;

await using var db = new CapstoneDbContext(options);
await db.Database.EnsureCreatedAsync();

var watchlist = new WatchlistService(db);

Console.WriteLine("Capstone Chatbot Watchlist");
Console.WriteLine("Commands: add | list | exit");

while (true)
{
    Console.Write("\n> ");
    var cmd = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (cmd is "exit" or "quit" or "q")
        break;

    if (cmd is "list")
    {
        var items = await watchlist.ListAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Watchlist is empty.");
            continue;
        }

        foreach (var item in items)
            Console.WriteLine($"- {item.Title} ({item.MediaType}) | Watched: {item.Watched} | Rating: {item.Rating}");

        continue;
    }

    if (cmd is "add")
    {
        Console.Write("Title: ");
        var title = Console.ReadLine() ?? "";

        Console.Write("Media Type (movie/tv): ");
        var mediaTypeText = Console.ReadLine() ?? "";
        MediaType mediaType = mediaTypeText.ToLowerInvariant() switch
        {
            "movie" => MediaType.Movie,
            "tv" => MediaType.TvShow,
            _ => MediaType.Unknown
        };

        Console.Write("Rating (1–5, halves allowed, optional): ");
        var ratingText = Console.ReadLine();

        decimal? rating = null;
        if (!string.IsNullOrWhiteSpace(ratingText) && decimal.TryParse(ratingText, out var parsed))
            rating = parsed;

        try
        {
            await watchlist.AddAsync(title, mediaType, rating);
            Console.WriteLine("Item added.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not add item: {ex.Message}");
        }

        continue;
    }

    Console.WriteLine("Unknown command. Try: add, list, exit");
}
