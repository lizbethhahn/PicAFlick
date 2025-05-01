using DotNetEnv;

namespace PicAFlick.Data
{
    public class TmdbApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiKey;
        private readonly string BaseUrl = "https://api.themoviedb.org/3/";

        public TmdbApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Fetch API key from environment variable
            ApiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY")
                     ?? throw new InvalidOperationException("TMDB_API_KEY not set");
        }

        public async Task<string> GetMovieByTitleAsync(string title)
        {
            // Construct the request URL
            string requestUrl = $"search/movie?api_key={ApiKey}&query={Uri.EscapeDataString(title)}";
            // Make the HTTP GET request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                // Read and return the response content as a string
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle error (e.g., log it, throw exception, etc.)
                return null;
            }
        }

        public async Task<string> GetTvShowByTitleAsync(string title)
        { 
            string requestUrl = $"search/tv?api_key={ApiKey}&query={Uri.EscapeDataString(title)}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                // Read and return the response content as a string
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle error (e.g., log it, throw exception, etc.)
                return null;
            }
        }
    }
}
