using CapstoneChatbot.App.Models;

namespace CapstoneChatbot.App.Services
{
    public class WatchlistAnalyzer
    {
        public WatchlistAnalysisResult Analyze(IEnumerable<WatchlistItemDto> watchlist)
        {
            var result = new WatchlistAnalysisResult
            {
                TotalCount = watchlist.Count(),
                WatchedCount = watchlist.Count(i => i.Watched),
                UnwatchedCount = watchlist.Count(i => !i.Watched),
                MovieCount = watchlist.Count(i => i.MediaType == Tmdb.Enums.MediaType.Movie),
                TvShowCount = watchlist.Count(i => i.MediaType == Tmdb.Enums.MediaType.TvShow),
                UnwatchedMovieCount = watchlist.Count(i => i.MediaType == Tmdb.Enums.MediaType.Movie && !i.Watched),
                UnwatchedTvShowCount = watchlist.Count(i => i.MediaType == Tmdb.Enums.MediaType.TvShow && !i.Watched),
                AllTitles = watchlist.Select(i => i.Title).ToList(),
                WatchedTitles = watchlist.Where(i => i.Watched).Select(i => i.Title).ToList(),
                UnwatchedTitles = watchlist.Where(i => !i.Watched).Select(i => i.Title).ToList(),
                HasAnyItems = watchlist.Any(),
                HasUnwatchedItems = watchlist.Any(i => !i.Watched),
            };
            result.CompletionPercentage = result.TotalCount > 0
                ? (double)result.WatchedCount / result.TotalCount * 100
                : 0;
            return result;
        }
    }
}
