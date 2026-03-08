using CapstoneChatbot.Tmdb.Enums;
using CapstoneChatbot.Tmdb.Models;
using System.Net.Http.Json;

namespace CapstoneChatbot.Tmdb.Clients
{
    public class TmdbClient : ITmdbClient
    {
        private readonly HttpClient _httpClient;

        public TmdbClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<TmdbSearchResult>> SearchAsync(
            string query,
            MediaType mediaType,
            CancellationToken cancellationToken = default)
        {
            var endpoint = mediaType switch
            {
                MediaType.Movie => $"search/movie?query={Uri.EscapeDataString(query)}",
                MediaType.TvShow => $"search/tv?query={Uri.EscapeDataString(query)}",
                _ => throw new ArgumentException("Media type must be Movie or TvShow.", nameof(mediaType))
            };

            var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponseDto>(
                endpoint,
                cancellationToken);

            if (response is null)
            {
                return Array.Empty<TmdbSearchResult>();
            }

            return response.Results
                .Select(item => new TmdbSearchResult
                {
                    TmdbId = item.Id,
                    Title = item.Title ?? item.Name ?? string.Empty,
                    MediaType = mediaType,
                    ReleaseDate = item.ReleaseDate ?? item.FirstAirDate,
                    Overview = item.Overview,
                    PosterPath = item.PosterPath
                })
                .ToList();
        }
    }
}