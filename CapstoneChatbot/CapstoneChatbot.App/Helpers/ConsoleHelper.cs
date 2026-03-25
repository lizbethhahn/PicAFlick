namespace CapstoneChatbot.App.Helpers;

public static class ConsoleHelper
{
    public const string CommandPrompt =
        "Available watchlist commands:\n> Commands: add | list | search | remove | watched | analyze | chat | exit";

    public static string WrapText(string text, int maxLineLength = 80)
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
}
