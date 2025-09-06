using PicAFlick.Domain.Enums;

namespace PicAFlick.Shared.Contracts
{
    public class WatchlistCreationDto
    {
        public int TmdbId { get; set; }
        public required string Title { get; set; } =  string.Empty;
        public required MediaType MediaType { get; set; }
        public string? Notes { get; set; } 
        
     // public decimal UserRating { get; set; }
    }
}
