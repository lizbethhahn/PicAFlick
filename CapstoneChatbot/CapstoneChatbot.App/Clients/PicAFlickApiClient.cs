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
    }
}