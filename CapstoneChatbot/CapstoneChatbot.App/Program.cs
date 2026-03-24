using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Data;
using CapstoneChatbot.App.Models;
using CapstoneChatbot.App.Services;
using CapstoneChatbot.Tmdb.Clients;
using CapstoneChatbot.Tmdb.Enums;
using CapstoneChatbot.Tmdb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using PicAFlick.Shared.Contracts;
using Sprache;

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

const string CommandPrompt = "Available watchlist commands:\n> Commands: add | list | search | remove | watched | analyze | chat | exit";

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
            var results = await tmdbApiClient.SearchAsync(query, mediaType);

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

    if (cmd is "analyze")
    {
        var items = await picAFlickApiClient.GetWatchlistAsync();
        var session = new ChatSession();

        var lines = items.Select(item =>
        {
            var releaseYear = item.ReleaseDate.HasValue && item.ReleaseDate.Value.Year > 1
                ? item.ReleaseDate.Value.Year.ToString()
                : "Unknown";
            return $"{item.Title} | {releaseYear} | {item.MediaType} | {item.Watched}";
        });

        var joined = string.Join(Environment.NewLine, lines);

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

            var prompt = $@"
            You are a movie and TV recommendation assistant.

            Analyze the user's watchlist and:
            1. Summarize patterns
            2. Recommend EXACTLY ONE item FROM the watchlist
            3. Explain why

            Rules:
            - Use ONLY the data provided
            - Only recommend titles that appear in the watchlist
            - Prefer unwatched items
            - If all items are watched, suggest one rewatch from the list
            - Do NOT suggest anything not in the list

            Formatting:
            - Return plain text only (no markdown)
            - Use simple headers like: Summary:, Recommendation:, Reasoning:
            - Use dots (•) for bullet points
            - Do not use #, *, or markdown syntax
            - Don't wrap text, keep width to 80 characters, including spaces

            Output format (strict):
            Summary:
            • ...
            Recommendation:
            Title | Year | MediaType
            Reasoning:
            • ...

            Follow the output format exactly. Do not add any extra text before or after.
            Return the recommended title exactly as it appears in the watchlist.

            Watchlist:
            {joined}
            ";

            var resultContext = await kernel.InvokePromptAsync(prompt);
            Console.WriteLine("\n--- Watchlist Analyzer ---\n");
            var aiReply = resultContext.ToString();

            Console.WriteLine(aiReply.Trim());

            Console.Write("\nType 'watch' to mark the recommended item as watched, or press Enter to continue: ");
            var userInput = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

            if (userInput == "watch")
            {
                var responseLines = aiReply
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim())
                    .ToList();

                var recIndex = responseLines.FindIndex(line =>
                    line.StartsWith("Recommendation:", StringComparison.OrdinalIgnoreCase));

                if (recIndex == -1)
                {
                    Console.WriteLine("Could not find a recommendation in the AI response.");
                }
                else
                {
                    var recommendationValue = responseLines[recIndex]["Recommendation:".Length..].Trim();

                    if (string.IsNullOrWhiteSpace(recommendationValue) && recIndex + 1 < responseLines.Count)
                    {
                        var nextLine = responseLines[recIndex + 1].Trim();

                        var isAnotherSectionHeader =
                            nextLine.StartsWith("Summary:", StringComparison.OrdinalIgnoreCase) ||
                            nextLine.StartsWith("Recommendation:", StringComparison.OrdinalIgnoreCase) ||
                            nextLine.StartsWith("Reasoning:", StringComparison.OrdinalIgnoreCase);

                        if (!isAnotherSectionHeader)
                        {
                            recommendationValue = nextLine;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(recommendationValue))
                    {
                        Console.WriteLine("The recommendation was empty or malformed.");
                    }
                    else
                    {
                        var parts = recommendationValue
                            .Split('|', StringSplitOptions.TrimEntries)
                            .ToList();

                        var titlePart = parts.Count > 0 ? parts[0] : "";
                        var yearPart = parts.Count > 1 ? parts[1] : "";
                        var mediaTypePart = parts.Count > 2 ? parts[2] : "";
                        var normalizedMediaTypePart = mediaTypePart.Replace(" ", "");

                        if (titlePart.StartsWith("Watch ", StringComparison.OrdinalIgnoreCase))
                        {
                            titlePart = titlePart.Substring("Watch ".Length).Trim();
                        }

                        int? parsedYear = null;
                        if (int.TryParse(yearPart, out var year))
                        {
                            parsedYear = year;
                        }

                        var exactMatches = items.Where(it =>
                            string.Equals(it.Title, titlePart, StringComparison.OrdinalIgnoreCase) &&
                            (!parsedYear.HasValue || it.ReleaseDate.HasValue && it.ReleaseDate.Value.Year == parsedYear.Value) &&
                            (string.IsNullOrWhiteSpace(mediaTypePart) ||
                             string.Equals(it.MediaType.ToString(), normalizedMediaTypePart, StringComparison.OrdinalIgnoreCase)))
                            .ToList();

                        var titleAndYearMatches = items.Where(it =>
                            string.Equals(it.Title, titlePart, StringComparison.OrdinalIgnoreCase) &&
                            (!parsedYear.HasValue || it.ReleaseDate.HasValue && it.ReleaseDate.Value.Year == parsedYear.Value))
                            .ToList();

                        var titleOnlyMatches = items.Where(it =>
                            string.Equals(it.Title, titlePart, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        var candidateMatches = exactMatches.Any()
                            ? exactMatches
                            : titleAndYearMatches.Any()
                                ? titleAndYearMatches
                                : titleOnlyMatches;

                        if (!candidateMatches.Any())
                        {
                            Console.WriteLine($"'{titlePart}' was not found in your watchlist.");
                        }
                        else if (candidateMatches.Count > 1)
                        {
                            Console.WriteLine(
                                $"More than one watchlist item matched '{titlePart}'. " +
                                "The recommendation was ambiguous, so nothing was marked as watched.");
                        }
                        else
                        {
                            var match = candidateMatches.Single();

                            if (match.Watched)
                            {
                                Console.WriteLine($"'{match.Title}' is already marked as watched.");
                            }
                            else
                            {
                                try
                                {
                                    await picAFlickApiClient.MarkAsWatchedAsync(match.Id);
                                    Console.WriteLine($"'{match.Title}' has been marked as watched.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Failed to mark as watched: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AI call failed: {ex.Message}");
            Console.WriteLine(joined);
        }

        Console.Write("\n> ");
        Console.WriteLine(CommandPrompt);
        continue;
    }

    if (cmd is "chat")
    {
        var session = new ChatSession();

        var items = await picAFlickApiClient.GetWatchlistAsync();

        if (items.Count == 0)
        {
            Console.WriteLine("Your watchlist is empty.");
            Console.Write("\n> ");
            Console.WriteLine(CommandPrompt);
            continue;
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
                        Console.WriteLine(WrapText(result.ToString()));
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

                    await picAFlickApiClient.AddToWatchlistAsync(new WatchlistCreationDto
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

                var movieResults = await tmdbApiClient.SearchAsync(titleToAdd, MediaType.Movie);
                var tvResults = await tmdbApiClient.SearchAsync(titleToAdd, MediaType.TvShow);

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
                Console.WriteLine(WrapText(reply));
                session.ConversationHistory.Add($"Bot: {reply}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI call failed: {ex.Message}");
                Console.WriteLine("Last user input:");
                Console.WriteLine(input);
            }
        }

        Console.Write("\n> ");
        Console.WriteLine(CommandPrompt);
        continue;
    }
}

static string WrapText(string text, int maxLineLength = 80)
{
    if (string.IsNullOrWhiteSpace(text))
        return text;

    var originalLines = text.Replace("\r\n", "\n").Split('\n');
    var wrappedLines = new List<string>();

    foreach (var originalLine in originalLines)
    {
        if (string.IsNullOrWhiteSpace(originalLine))
        {
            wrappedLines.Add(string.Empty);
            continue;
        }

        var words = originalLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var currentLine = "";

        foreach (var word in words)
        {
            if (currentLine.Length == 0)
            {
                currentLine = word;
            }
            else if ((currentLine.Length + 1 + word.Length) <= maxLineLength)
            {
                currentLine += " " + word;
            }
            else
            {
                wrappedLines.Add(currentLine);
                currentLine = word;
            }
        }

        if (!string.IsNullOrWhiteSpace(currentLine))
        {
            wrappedLines.Add(currentLine);
        }
    }

    return string.Join(Environment.NewLine, wrappedLines);
}
