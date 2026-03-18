namespace CapstoneChatbot.App.Models
{
    public class WatchlistAnalysisResult
    {
        public int TotalCount { get; set; }
        public int WatchedCount { get; set; }
        public int UnwatchedCount { get; set; }
        public int MovieCount { get; set; }
        public int TvShowCount { get; set; }
        public int UnwatchedMovieCount { get; set; }
        public int UnwatchedTvShowCount { get; set; }
        public List<string> AllTitles { get; set; } = new List<string>();
        public List<string> WatchedTitles { get; set; } = new List<string>();
        public List<string> UnwatchedTitles { get; set; } = new List<string>();
        public bool HasAnyItems { get; set; }
        public bool HasUnwatchedItems { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
