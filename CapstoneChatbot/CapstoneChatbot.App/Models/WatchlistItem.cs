using CapstoneChatbot.Tmdb.Enums;
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

        public MediaType MediaType { get; set; }

        public int? TmdbId { get; set; }

        public bool Watched { get; set; }

        public decimal? Rating { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
