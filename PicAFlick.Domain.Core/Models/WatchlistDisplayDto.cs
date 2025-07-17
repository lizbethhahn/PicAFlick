using PicAFlick.Domain.Core.Enums;

namespace PicAFlick.Domain.Core.Models
{
    public class WatchlistDisplayDto
    {
        public MediaType MediaType { get; set; }
        public string? Notes { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public decimal? Rating { get; set; }
        public int? ReleaseYear { get; set; }
        public int TmdbId { get; set; }
        public required string Title { get; set; }
        public bool Watched { get; set; }
    }
}
