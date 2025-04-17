using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PicAFlick.Data
{
    public class TmdbApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiKey = "1b9fad7d7035a0af07bea1e5c5d15b9f";
        private readonly string BaseUrl = "https://api.themoviedb.org/3/";

        public TmdbApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
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
