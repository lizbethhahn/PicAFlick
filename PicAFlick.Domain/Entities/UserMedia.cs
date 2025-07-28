using PicAFlick.Domain.Enums;

namespace PicAFlick.Domain.Entity
{
    public class UserMedia
    {
        public int Id { get; set; } // Primary key

        public int TmdbId { get; set; } // TMDb's ID for movie or TV show

        public string Title { get; set; } = string.Empty;

        public MediaType MediaType { get; set; } // Enum: Movie or TV

        public bool Watched { get; set; }

        public string? Notes { get; set; }

        public decimal? UserRating { get; set; }
    }
}
