using PicAFlick.Domain.Enums;

namespace PicAFlick.Shared.Contracts
{
    public class WatchlistCreationDto
    {
        // Assigned by controller
        public required string UserId { get; set; } = string.Empty;

        // Required Fields
        public required string Title { get; set; } =  string.Empty;
        public required MediaType MediaType { get; set; }

        // Tmdb metadata
        public int TmdbId { get; set; }
        public string? PosterPath { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Overview {  get; set; }

        // details added by user
        public string? Notes { get; set; }  
    }
}
