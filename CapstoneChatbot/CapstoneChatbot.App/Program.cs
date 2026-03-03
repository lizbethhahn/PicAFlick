using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Services;
using Microsoft.EntityFrameworkCore;

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
        var mediaType = Console.ReadLine() ?? "";

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
