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
                if (session.PendingAddResults.Count == 0)
                {
                    session.IsWaitingForAddSelection = false;
                    Console.WriteLine("Bot: Add selection was cleared.");
                    continue;
                }

                if (input.Equals("list", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("watchlist", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("what is in my watchlist", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("what's in my watchlist", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("what is in my list", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("what's in my list", StringComparison.OrdinalIgnoreCase))
                {
                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;

                    var watchlist = await picClient.GetWatchlistAsync();

                    if (watchlist.Count == 0)
                    {
                        Console.WriteLine("Bot: Your watchlist is empty.");
                        continue;
                    }

                    var unwatchedMovies = watchlist
                        .Where(x => !x.Watched && x.MediaType == MediaType.Movie)
                        .ToList();

                    var watchedMovies = watchlist
                        .Where(x => x.Watched && x.MediaType == MediaType.Movie)
                        .ToList();

                    var unwatchedTv = watchlist
                        .Where(x => !x.Watched && x.MediaType == MediaType.TvShow)
                        .ToList();

                    var watchedTv = watchlist
                        .Where(x => x.Watched && x.MediaType == MediaType.TvShow)
                        .ToList();

                    Console.WriteLine("Bot:");
                    Console.WriteLine("Your watchlist looks pretty great! Here's a breakdown:\n");

                    if (unwatchedMovies.Any())
                    {
                        Console.WriteLine("Unwatched Movies:");
                        foreach (var item in unwatchedMovies)
                        {
                            Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                        }
                        Console.WriteLine();
                    }

                    if (watchedMovies.Any())
                    {
                        Console.WriteLine("Watched Movies:");
                        foreach (var item in watchedMovies)
                        {
                            Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                        }
                        Console.WriteLine();
                    }

                    if (unwatchedTv.Any())
                    {
                        Console.WriteLine("Unwatched TV Shows:");
                        foreach (var item in unwatchedTv)
                        {
                            Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                        }
                        Console.WriteLine();
                    }

                    if (watchedTv.Any())
                    {
                        Console.WriteLine("Watched TV Shows:");
                        foreach (var item in watchedTv)
                        {
                            Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                        }
                        Console.WriteLine();
                    }

                    continue;
                }
                else if (input.StartsWith("add ", StringComparison.OrdinalIgnoreCase) &&
                         int.TryParse(input[4..].Trim(), out var addSelectionNumber))
                {
                    if (addSelectionNumber < 1 || addSelectionNumber > session.PendingAddResults.Count)
                    {
                        Console.WriteLine($"Bot: Please choose a number between 1 and {session.PendingAddResults.Count}.");
                        continue;
                    }

                    var chosenResult = session.PendingAddResults[addSelectionNumber - 1];

                    DateTime? releaseDate = null;
                    if (!string.IsNullOrWhiteSpace(chosenResult.ReleaseDate) &&
                        DateTime.TryParse(chosenResult.ReleaseDate, out var parsedReleaseDate))
                    {
                        releaseDate = parsedReleaseDate;
                    }

                    await picClient.AddToWatchlistAsync(new WatchlistCreationDto
                    {
                        Title = chosenResult.Title,
                        MediaType = (PicAFlick.Domain.Enums.MediaType)chosenResult.MediaType,
                        TmdbId = chosenResult.TmdbId,
                        ReleaseDate = releaseDate
                    });

                    Console.WriteLine($"Bot: Added '{chosenResult.Title}' to your watchlist.");

                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;
                    continue;
                }
                else if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                {
                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;
                    Console.WriteLine("Bot: Canceled. If you want to add something, start again with: add <movie title>.");
                    continue;
                }
                else if ((input.StartsWith("add ", StringComparison.OrdinalIgnoreCase) &&
                          !int.TryParse(input[4..].Trim(), out _)) ||
                         (input.StartsWith("mark ", StringComparison.OrdinalIgnoreCase) &&
                          input.EndsWith(" as watched", StringComparison.OrdinalIgnoreCase)))
                {
                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;
                }
                else if (input.StartsWith("who starred in ", StringComparison.OrdinalIgnoreCase) ||
                         input.StartsWith("tell me who starred in ", StringComparison.OrdinalIgnoreCase))
                {
                    var prefix = input.StartsWith("tell me who starred in ", StringComparison.OrdinalIgnoreCase)
                        ? "tell me who starred in "
                        : "who starred in ";

                    var numberText = input[prefix.Length..].Trim().TrimEnd('?');

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

                        string year = "Unknown year";

                        if (!string.IsNullOrWhiteSpace(selected.ReleaseDate) &&
                            DateTime.TryParse(selected.ReleaseDate, out var parsedActorDate))
                        {
                            year = parsedActorDate.Year.ToString();
                        }

                        var actorPrompt = $@"
                        You are a movie and TV assistant.

                        Tell me the main actors in this exact title.
                        Use the year and media type to disambiguate remakes, alternate versions, and similarly named titles.

                        Title: {selected.Title}
                        Year: {year}
                        MediaType: {selected.MediaType}
                        TmdbId: {selected.TmdbId}

                        Respond with a short numbered list of the main actors for this exact title only.
                        If the title is ambiguous, prefer the match that exactly fits the year, media type, and TMDb id above.
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
                else if (input.StartsWith("tell me about ", StringComparison.OrdinalIgnoreCase))
                {
                    var numberText = input["tell me about ".Length..].Trim().TrimEnd('?');

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
                else if (int.TryParse(input, out var selection) &&
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
                else
                {
                    Console.WriteLine("Bot: Choose a number to add, or type:");
                    Console.WriteLine("  - tell me about <number>");
                    Console.WriteLine("  - who starred in <number>");
                    Console.WriteLine("  - list (view your watchlist)");
                    Console.WriteLine("  - cancel (go back)");
                    continue;
                }
            }

            if (input.Equals("list", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("watchlist", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("what is in my watchlist", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("what's in my watchlist", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("what is in my list", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("what's in my list", StringComparison.OrdinalIgnoreCase))
            {
                var watchlist = await picClient.GetWatchlistAsync();

                if (watchlist.Count == 0)
                {
                    Console.WriteLine("Bot: Your watchlist is empty.");
                    continue;
                }

                var unwatchedMovies = watchlist
                    .Where(x => !x.Watched && x.MediaType == MediaType.Movie)
                    .ToList();

                var watchedMovies = watchlist
                    .Where(x => x.Watched && x.MediaType == MediaType.Movie)
                    .ToList();

                var unwatchedTv = watchlist
                    .Where(x => !x.Watched && x.MediaType == MediaType.TvShow)
                    .ToList();

                var watchedTv = watchlist
                    .Where(x => x.Watched && x.MediaType == MediaType.TvShow)
                    .ToList();

                Console.WriteLine("Bot:");
                Console.WriteLine("Your watchlist looks pretty great! Here's a breakdown:\n");

                if (unwatchedMovies.Any())
                {
                    Console.WriteLine("Unwatched Movies:");
                    foreach (var item in unwatchedMovies)
                    {
                        Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                    }
                    Console.WriteLine();
                }

                if (watchedMovies.Any())
                {
                    Console.WriteLine("Watched Movies:");
                    foreach (var item in watchedMovies)
                    {
                        Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                    }
                    Console.WriteLine();
                }

                if (unwatchedTv.Any())
                {
                    Console.WriteLine("Unwatched TV Shows:");
                    foreach (var item in unwatchedTv)
                    {
                        Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                    }
                    Console.WriteLine();
                }

                if (watchedTv.Any())
                {
                    Console.WriteLine("Watched TV Shows:");
                    foreach (var item in watchedTv)
                    {
                        Console.WriteLine($"- {item.Title} ({item.ReleaseDate?.Year})");
                    }
                    Console.WriteLine();
                }

                continue;
            }

            if (input.StartsWith("mark ", StringComparison.OrdinalIgnoreCase) &&
                input.EndsWith(" as watched", StringComparison.OrdinalIgnoreCase))
            {
                const string prefix = "mark ";
                const string suffix = " as watched";

                // Guard: Ensure there's enough length for both prefix and suffix
                if (input.Length <= prefix.Length + suffix.Length)
                {
                    Console.WriteLine("Bot: Please specify a title to mark as watched.");
                    continue;
                }

                var titleToMark = input[prefix.Length..^suffix.Length].Trim();

                if (string.IsNullOrWhiteSpace(titleToMark))
                {
                    Console.WriteLine("Bot: Please specify a title to mark as watched.");
                    continue;
                }

                var watchlist = await picClient.GetWatchlistAsync();

                var matches = watchlist
                    .Where(item => item.Title.Contains(titleToMark, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matches.Count == 0)
                {
                    Console.WriteLine($"Bot: I couldn't find '{titleToMark}' in your watchlist.");
                    continue;
                }

                if (matches.Count > 1)
                {
                    Console.WriteLine("Bot: I found more than one match in your watchlist:");
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var year = matches[i].ReleaseDate?.Year.ToString() ?? "Unknown year";
                        Console.WriteLine($"{i + 1}. {matches[i].Title} ({year}) [{matches[i].MediaType}]");
                    }

                    Console.WriteLine("Bot: I found more than one match. Try 'list' and mark the correct title as watched from there.");
                    continue;
                }

                var match = matches[0];

                await picClient.MarkAsWatchedAsync(match.Id);

                Console.WriteLine($"Bot: Marked '{match.Title}' as watched.");
                continue;
            }

            if (input.StartsWith("add ", StringComparison.OrdinalIgnoreCase))
            {
                var addValue = input[4..].Trim();

                if (string.IsNullOrWhiteSpace(addValue))
                {
                    Console.WriteLine("Bot: Please type a title after 'add'.");
                    continue;
                }

                // If we already have pending numbered results, allow: add 3
                if (session.IsWaitingForAddSelection &&
                    session.PendingAddResults.Count > 0 &&
                    int.TryParse(addValue, out var selectedNumber))
                {
                    if (selectedNumber < 1 || selectedNumber > session.PendingAddResults.Count)
                    {
                        Console.WriteLine($"Bot: Please choose a number between 1 and {session.PendingAddResults.Count}.");
                        continue;
                    }

                    var chosenResult = session.PendingAddResults[selectedNumber - 1];

                    DateTime? releaseDate = null;
                    if (!string.IsNullOrWhiteSpace(chosenResult.ReleaseDate) &&
                        DateTime.TryParse(chosenResult.ReleaseDate, out var parsedReleaseDate))
                    {
                        releaseDate = parsedReleaseDate;
                    }

                    await picClient.AddToWatchlistAsync(new WatchlistCreationDto
                    {
                        Title = chosenResult.Title,
                        MediaType = (PicAFlick.Domain.Enums.MediaType)chosenResult.MediaType,
                        TmdbId = chosenResult.TmdbId,
                        ReleaseDate = releaseDate
                    });

                    Console.WriteLine($"Bot: Added '{chosenResult.Title}' to your watchlist.");

                    session.PendingAddResults.Clear();
                    session.IsWaitingForAddSelection = false;
                    continue;
                }

                Console.WriteLine($"Bot: Okay — I’ll try to add '{addValue}'.");

                var movieResults = await tmdbClient.SearchAsync(addValue, MediaType.Movie);
                var tvResults = await tmdbClient.SearchAsync(addValue, MediaType.TvShow);

                var results = new List<TmdbSearchResult>();

                if (movieResults != null)
                {
                    results.AddRange(movieResults);
                }

                if (tvResults != null)
                {
                    results.AddRange(tvResults);
                }

                if (results.Count == 0)
                {
                    Console.WriteLine($"Bot: I couldn't find any matches for '{addValue}'.");
                    Console.WriteLine("Try a simpler title, like: add Little Women");
                    continue;
                }

                Console.WriteLine("Bot: I found these matches. Please choose one by number:");

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

                Console.WriteLine();

                session.PendingAddResults.Clear();
                session.PendingAddResults.AddRange(results);
                session.IsWaitingForAddSelection = true;

                Console.WriteLine("Bot: Choose a number to add, or type:");
                Console.WriteLine("  - add <number>");
                Console.WriteLine("  - tell me about <number>");
                Console.WriteLine("  - who starred in <number>");
                Console.WriteLine("  - list (view your watchlist)");
                Console.WriteLine("  - cancel (go back)");
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

                var currentWatchlist = await picClient.GetWatchlistAsync();
                var joinedWatchlist = string.Join(Environment.NewLine, currentWatchlist.Select(item =>
                    $"{item.Title} | {item.ReleaseDate?.Year} | {item.MediaType} | Watched: {item.Watched}"));

                var promptchat = $@"
                You are a movie and TV assistant helping the user explore their watchlist.

                The user has this watchlist:
                {joinedWatchlist}

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