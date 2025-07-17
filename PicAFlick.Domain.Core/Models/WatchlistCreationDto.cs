using PicAFlick.Domain.Core.Enums;

namespace PicAFlick.Domain.Core.Models
{
    public class WatchlistCreationDto
    {
        // Required Fields
        public required string Title { get; set; } =  string.Empty;
        public required MediaType MediaType { get; set; }

        // Tmdb metadata
        public int? TmdbId { get; set; }
        public string? PosterPath { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Overview {  get; set; }

        // details added by user
        public string? Notes { get; set; }  
    }
}
