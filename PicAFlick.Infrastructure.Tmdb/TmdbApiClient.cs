using System.Text.Json;
using PicAFlick.Infrastructure.Tmdb.Models;

namespace PicAFlick.Infrastructure.Tmdb
{
    public class TmdbApiClient : ITmdbApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.themoviedb.org/3/";

        public TmdbApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<TmdbMovieSearchResponseDto?> GetMovieByTitleAsync(string query)
        {
            // Construct the request URL
            string requestUrl = $"search/movie?query={Uri.EscapeDataString(query)}";
            // Make the HTTP GET request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Read and return the response content as a string
                return JsonSerializer.Deserialize<TmdbMovieSearchResponseDto>(json);
            }
            else
            {
                // Handle error (e.g., log it, throw exception, etc.)
                return null;
            }
        }

        public async Task<TmdbTvShowSearchResponseDto?> GetTvShowByTitleAsync(string query)
        { 
            string requestUrl = $"search/tv?query={Uri.EscapeDataString(query)}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Read and return the response content as a string
                return JsonSerializer.Deserialize<TmdbTvShowSearchResponseDto>(json);
            }
            else
            {
                // Handle error (e.g., log it, throw exception, etc.)
                return null;
            }
        }

        public async Task<string?> GetMovieCreditsAsync(int tmdbId)
        {
            string requestUrl = $"movie/{tmdbId}/credits";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}