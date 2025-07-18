using PicAFlick.Domain.Core.Enums;

namespace PicAFlick.Domain.Core.Models
{
    public class WatchlistDisplayDto
    {   
        // Id set by Db
        public int Id { get; set; }

        // Required fields
        public required string UserId { get; set; }
        public required string Title { get; set; }
        public required MediaType MediaType { get; set; }

        // Tmdb metadata
        public int? TmdbId { get; set; }
        public string? PosterPath { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Overview { get; set; }

        // details added by user
        public string? Notes { get; set; }
        public bool Watched { get; set; }
    }
}
