using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Helpers;
using CapstoneChatbot.App.Models;
using CapstoneChatbot.Tmdb.Clients;
using CapstoneChatbot.Tmdb.Enums;
using CapstoneChatbot.Tmdb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using PicAFlick.Shared.Contracts;

namespace CapstoneChatbot.App.Commands;

public static class ChatCommand
{
    public static async Task ExecuteAsync(
        IPicAFlickApiClient picClient,
        ITmdbApiClient tmdbClient,
        IConfiguration configuration)
    {
        var session = new ChatSession();

        var items = await picClient.GetWatchlistAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Your watchlist is empty.");
            Console.WriteLine();
            Console.WriteLine(ConsoleHelper.CommandPrompt);
            return;
        }

        var joined = string.Join(Environment.NewLine, items.Select(item =>
            $"{item.Title} | {item.ReleaseDate?.Year} | {item.MediaType} | Watched: {item.Watched}"));

        Console.WriteLine("Chat mode started. Type 'exit' to leave chat.");

        while (true)
        {
            Console.Write("\nYou: ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            if (session.IsWaitingForAddSelection)
            {
                if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                {
                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;
                    Console.WriteLine("Bot: Add canceled.");
                    continue;
                }

                if (input.StartsWith("who starred in ", StringComparison.OrdinalIgnoreCase))
                {
                    var numberText = input["who starred in ".Length..].Trim();

                    if (int.TryParse(numberText, out var actorSelection) &&
                        actorSelection >= 1 &&
                        actorSelection <= session.PendingAddResults.Count)
                    {
                        var selected = session.PendingAddResults[actorSelection - 1];

                        Console.WriteLine($"Bot: Let me check who stars in {selected.Title}...");

                        var githubToken = configuration["GithubModels:ApiKey"]
                          ?? throw new InvalidOperationException("GithubModels:ApiKey user secret is missing.");

                        var kernelBuilder = Kernel.CreateBuilder()
                            .AddOpenAIChatCompletion(
                                modelId: "openai/gpt-4o",
                                apiKey: githubToken,
                                endpoint: new Uri("https://models.github.ai/inference")
                            );

                        var kernel = kernelBuilder.Build();

                        var actorPrompt = $@"
                        You are a movie and TV assistant.

                        Tell me the main actors in this title:

                        Title: {selected.Title}
                        MediaType: {selected.MediaType}

                        Respond with a short list of main actors.
                        ";

                        var result = await kernel.InvokePromptAsync(actorPrompt);
                        Console.WriteLine(ConsoleHelper.WrapText(result.ToString()));
                    }
                    else
                    {
                        Console.WriteLine("Bot: Please choose a valid result number.");
                    }

                    continue;
                }

                if (input.StartsWith("tell me about ", StringComparison.OrdinalIgnoreCase))
                {
                    var numberText = input["tell me about ".Length..].Trim();

                    if (int.TryParse(numberText, out var detailSelection) &&
                        detailSelection >= 1 &&
                        detailSelection <= session.PendingAddResults.Count)
                    {
                        var selected = session.PendingAddResults[detailSelection - 1];

                        string year = "Unknown year";

                        if (!string.IsNullOrWhiteSpace(selected.ReleaseDate) &&
                            DateTime.TryParse(selected.ReleaseDate, out var parsedDetailDate))
                        {
                            year = parsedDetailDate.Year.ToString();
                        }

                        var overview = string.IsNullOrWhiteSpace(selected.Overview)
                            ? "No overview available."
                            : selected.Overview;

                        Console.WriteLine($"Bot: {selected.Title} ({year}) [{selected.MediaType}]");
                        Console.WriteLine(overview);
                    }
                    else
                    {
                        Console.WriteLine("Bot: Please choose a valid result number.");
                    }

                    continue;
                }

                if (int.TryParse(input, out var selection) &&
                    selection >= 1 &&
                    selection <= session.PendingAddResults.Count)
                {
                    var chosenResult = session.PendingAddResults[selection - 1];

                    DateTime? releaseDate = null;

                    if (!string.IsNullOrWhiteSpace(chosenResult.ReleaseDate) &&
                        DateTime.TryParse(chosenResult.ReleaseDate, out var parsedSelectedDate))
                    {
                        releaseDate = parsedSelectedDate;
                    }

                    await picClient.AddToWatchlistAsync(new WatchlistCreationDto
                    {
                        Title = chosenResult.Title,
                        MediaType = (PicAFlick.Domain.Enums.MediaType)chosenResult.MediaType,
                        TmdbId = chosenResult.TmdbId,
                        ReleaseDate = releaseDate
                    });

                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;

                    Console.WriteLine($"Bot: Added '{chosenResult.Title}' to your watchlist.");
                    continue;
                }

                Console.WriteLine("Bot: Type a number to add one of the results, or type 'tell me about 2', 'who starred in 5', or 'cancel'.");
                continue;
            }

            if (input.StartsWith("add ", StringComparison.OrdinalIgnoreCase))
            {
                var titleToAdd = input[4..].Trim();

                if (string.IsNullOrWhiteSpace(titleToAdd))
                {
                    Console.WriteLine("Bot: Please type a title after 'add'.");
                    continue;
                }

                Console.WriteLine($"Bot: Okay — I’ll try to add '{titleToAdd}'.");

                var movieResults = await tmdbClient.SearchAsync(titleToAdd, MediaType.Movie);
                var tvResults = await tmdbClient.SearchAsync(titleToAdd, MediaType.TvShow);

                var results = new List<TmdbSearchResult>();

                if (movieResults != null)
                {
                    results.AddRange(movieResults);
                }

                if (tvResults != null)
                {
                    results.AddRange(tvResults);
                }

                Console.WriteLine("Bot: I found these matches:");

                for (int i = 0; i < results.Count; i++)
                {
                    var result = results[i];

                    string year = "Unknown year";

                    if (!string.IsNullOrWhiteSpace(result.ReleaseDate) &&
                        DateTime.TryParse(result.ReleaseDate, out var parsedReleaseDate))
                    {
                        year = parsedReleaseDate.Year.ToString();
                    }

                    Console.WriteLine($"{i + 1}. {result.Title} ({year}) [{result.MediaType}]");
                }

                session.PendingAddResults.Clear();
                session.PendingAddResults.AddRange(results);
                session.IsWaitingForAddSelection = true;

                Console.WriteLine("Bot: Type a number to add one of the results, or type 'tell me about 2', 'who starred in 5', or 'cancel'.");
                continue;
            }

            session.ConversationHistory.Add($"User: {input}");
            var conversationHistory = string.Join(Environment.NewLine, session.ConversationHistory);

            try
            {
                var githubToken = configuration["GithubModels:ApiKey"]
                                  ?? throw new InvalidOperationException("GithubModels:ApiKey user secret is missing.");

                var kernelBuilder = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(
                        modelId: "openai/gpt-4o",
                        apiKey: githubToken,
                        endpoint: new Uri("https://models.github.ai/inference")
                    );

                var kernel = kernelBuilder.Build();

                var promptchat = $@"
                You are a movie and TV assistant helping the user explore their watchlist.

                The user has this watchlist:
                {joined}

                Conversation so far:
                {conversationHistory}

                Rules:
                - Be conversational and helpful.
                - You may discuss movies, shows, actors, genres, and recommendations.
                - Do not claim you added, removed, or updated anything in the watchlist.
                - Only the application can perform watchlist actions.
                - If the user wants to add something, tell them to use: add <title>
                - If the user wants something marked watched, do not claim it happened unless the application confirms it.
                - Prefer titles from the user's watchlist when making recommendations, unless the user is clearly asking more generally.

                Respond in plain text only.
                ";

                var result = await kernel.InvokePromptAsync(promptchat);
                var reply = result.ToString();

                Console.WriteLine("Bot:");
                Console.WriteLine(ConsoleHelper.WrapText(reply));
                session.ConversationHistory.Add($"Bot: {reply}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI call failed: {ex.Message}");
                Console.WriteLine("Last user input:");
                Console.WriteLine(input);
            }
        }

        Console.WriteLine();
        Console.WriteLine(ConsoleHelper.CommandPrompt);
        return;
    }
}