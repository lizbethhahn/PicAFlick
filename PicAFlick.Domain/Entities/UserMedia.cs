using PicAFlick.Domain.Enums;

namespace PicAFlick.Domain.Entities
{
    public class UserMedia
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }            // UNIQUE
        public MediaType MediaType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Overview { get; set; }       // from TMDb
        public string? PosterPath { get; set; }     // TMDb gives relative path (e.g., "/abc.jpg")
        public DateTime? ReleaseDate { get; set; }

     // public DateTime LastSyncedUtc { get; set; } = DateTime.UtcNow;
    }
}
