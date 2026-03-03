using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneChatbot.App.Models
{
    public class WatchlistItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string MediaType { get; set; } = string.Empty; // "movie" or "tv"

        public int? TmdbId { get; set; }

        public bool Watched { get; set; }

        public decimal? Rating { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
