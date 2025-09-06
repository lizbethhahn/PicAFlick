using PicAFlick.Domain.Enums;

namespace PicAFlick.Domain.Entities
{
    public class WatchlistItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public int UserMediaId { get; set; }
        public UserMedia? UserMedia { get; set; } 
        public string? Notes { get; set; }
        public bool Watched { get; set; }

     // public DateTime AddedAtUtc { get; set; } = DateTime.UtcNow;

     // public decimal? Rating { get; set; }  // User rating 1-10
    }
}