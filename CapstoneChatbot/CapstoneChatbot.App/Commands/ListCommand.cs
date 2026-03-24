using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Helpers;

namespace CapstoneChatbot.App.Commands;
public static class ListCommand
{
    public static async Task ExecuteAsync(IPicAFlickApiClient picClient)
    {
        var items = await picClient.GetWatchlistAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Watchlist is empty.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine("----Here is your watchlist----");
            var releaseYear = item.ReleaseDate.HasValue && item.ReleaseDate.Value.Year > 1
                ? item.ReleaseDate.Value.Year.ToString()
                : "Unknown";

            Console.WriteLine($"{item.Title} - {releaseYear}  ({item.MediaType}) | Watched: {item.Watched} | TmdbId: {item.TmdbId}");
        }

        Console.WriteLine();
        Console.WriteLine(ConsoleHelper.CommandPrompt);
        return;
    }
}
