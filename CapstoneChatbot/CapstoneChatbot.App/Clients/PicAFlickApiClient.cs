using PicAFlick.Shared.Contracts;
using System.Net.Http.Json;

namespace CapstoneChatbot.App.Clients
{
    public class PicAFlickApiClient : IPicAFlickApiClient
    {
        private readonly HttpClient _httpClient;

        public PicAFlickApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WatchlistItemDto>> GetWatchlistAsync()
        {
            var watchlist = await _httpClient.GetFromJsonAsync<List<WatchlistItemDto>>(
                "https://localhost:7043/api/Watchlist"
            );

            return watchlist ?? new List<WatchlistItemDto>();
        }

        public async Task AddToWatchlistAsync(WatchlistCreationDto item)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Watchlist", item);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to add item. Status: {response.StatusCode}");
            }
        }

        public async Task MarkAsWatchedAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Watchlist/{id}/watched", null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to mark item as watched. Status: {response.StatusCode}");
            }
        }

        public async Task RemoveFromWatchlistAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Watchlist/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to remove item from watchlist. Status: {response.StatusCode}");
            }
        }
    }
}