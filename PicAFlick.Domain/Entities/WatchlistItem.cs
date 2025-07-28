using PicAFlick.Domain.Entity;
using PicAFlick.Domain.Enums;

namespace PicAFlick.Domain.Entities
{
    public class WatchlistItem
    {
        public int Id { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateWatched { get; set; }
        public MediaType MediaType { get; set; }
        public string? Notes { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public int? ReleaseYear { get; set; }
        public decimal? Rating { get; set; }
        public string? Title { get; set; }
        public int TmdbId { get; set; }
        public UserMedia UserMedia { get; set; } = null!;
        public int UserMediaId { get; set; }
        public required string UserId { get; set; } // UserId needs to be a string and strings aren't nullable; UserId is required.
        public bool Watched { get; set; } = false;
    }
}