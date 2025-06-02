using Domain.DTOs;
using PicAFlick.Domain.Services;
using System.Text.Json;

namespace PicAFlick.Data
{
    public class TmdbApiClient : ITmdbApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.themoviedb.org/3/";

        public TmdbApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Fetch API key from environment variable
            //_apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY")
            //         ?? throw new InvalidOperationException("TMDB_API_KEY not set");
        }

        public async Task<TmdbMovieSearchResponse> GetMovieByTitleAsync(string query, int page = 1)
        {
            // Construct the request URL
            string requestUrl = $"search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&page={page}";
            // Make the HTTP GET request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Read and return the response content as a string
                return JsonSerializer.Deserialize<TmdbMovieSearchResponse>(json);
            }
            else
            {
                // Handle error (e.g., log it, throw exception, etc.)
                return null;
            }
        }

        public async Task<TmdbMovieSearchResponse> GetTvShowByTitleAsync(string title)
        { 
            string requestUrl = $"search/tv?api_key={_apiKey}&query={Uri.EscapeDataString(title)}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Read and return the response content as a string
                return JsonSerializer.Deserialize<TmdbMovieSearchResponse>(json);
            }
            else
            {
                // Handle error (e.g., log it, throw exception, etc.)
                return null;
            }
        }
    }
}