using System.Text.Json.Serialization;

namespace PicAFlick.ExternalDtos
{
    public class TmdbMovieSearchResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<TmdbMovie>? Results { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }

    public class TmdbTvShowSearchResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<TmdbTvShow>? Results { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}