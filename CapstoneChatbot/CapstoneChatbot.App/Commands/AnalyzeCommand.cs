using CapstoneChatbot.App.Clients;
using CapstoneChatbot.App.Helpers;
using CapstoneChatbot.App.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace CapstoneChatbot.App.Commands;

public static class AnalyzeCommand
{
    public static async Task ExecuteAsync(
        IPicAFlickApiClient picClient,
        IConfiguration configuration)
    {
        var items = await picClient.GetWatchlistAsync();
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
                                    await picClient.MarkAsWatchedAsync(match.Id);
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

        Console.WriteLine(); ;
        Console.WriteLine(ConsoleHelper.CommandPrompt);
        return;
    }

}
