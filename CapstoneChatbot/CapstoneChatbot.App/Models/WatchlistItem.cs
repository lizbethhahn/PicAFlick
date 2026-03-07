using CapstoneChatbot.Tmdb.Enums;

namespace CapstoneChatbot.App.Models
{
    public class WatchlistItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public MediaType MediaType { get; set; }

        public int? TmdbId { get; set; }

        public bool Watched { get; set; }

        public decimal? Rating { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
