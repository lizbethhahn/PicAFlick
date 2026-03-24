using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Helpers;

namespace CapstoneChatbot.App.Commands;
public static class RemoveCommand
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

            Console.WriteLine($"{i + 1}. {item.Title} - {releaseYear} ({item.MediaType})");
        }

        Console.WriteLine();
        Console.Write("Enter a number to remove an item, or press Enter to return to the command prompt: ");
        var removeInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(removeInput))
        {
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        if (!int.TryParse(removeInput, out int removeSelection))
        {
            Console.WriteLine("Invalid selection.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        if (removeSelection < 1 || removeSelection > items.Count)
        {
            Console.WriteLine("Selection out of range.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        var itemToRemove = items[removeSelection - 1];

        try
        {
            await picClient.RemoveFromWatchlistAsync(itemToRemove.Id);
            Console.WriteLine("Item removed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not remove item: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine(ConsoleHelper.CommandPrompt);
        return;
    }
}
