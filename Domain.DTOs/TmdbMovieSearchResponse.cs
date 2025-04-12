using PicAFlick.Domain.Entities;
using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class TmdbMovieSearchResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<Movie>? Results { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}
