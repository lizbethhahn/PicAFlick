using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Helpers;

namespace CapstoneChatbot.App.Commands;
public static class WatchedCommand
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

        Console.WriteLine("Watchlist:");
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            var releaseYear = item.ReleaseDate.HasValue
                ? item.ReleaseDate.Value.Year.ToString()
                : "Unknown";

            Console.WriteLine($"{i + 1}. {item.Title} - {releaseYear} ({item.MediaType}) | Watched: {item.Watched}");
        }

        Console.WriteLine();
        Console.Write("Enter a number to mark an item as watched, or press Enter to return to the command prompt: ");
        var watchedInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(watchedInput))
        {
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        if (!int.TryParse(watchedInput, out int watchedSelection))
        {
            Console.WriteLine("Invalid selection.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        if (watchedSelection < 1 || watchedSelection > items.Count)
        {
            Console.WriteLine("Selection out of range.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        var itemToMarkWatched = items[watchedSelection - 1];

        try
        {
            await picClient.MarkAsWatchedAsync(itemToMarkWatched.Id);
            Console.WriteLine("Item marked as watched.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not mark item as watched: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine(ConsoleHelper.CommandPrompt);
        return;
    }
}
