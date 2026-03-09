using PicAFlick.Domain.Enums;

namespace PicAFlick.Shared.Contracts
{
    public class WatchlistDisplayDto
    {   
        // Id set by Db
        public int Id { get; set; }

        // Required fields
        public required string Title { get; set; } = string.Empty;
        public required MediaType MediaType { get; set; } = MediaType.Unknown;

        // Tmdb metadata
        public int? TmdbId { get; set; }
        public string? PosterPath { get; set; }
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }

        // details added by user
        public string? Notes { get; set; }
        public bool Watched { get; set; }
    }
}
