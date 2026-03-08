using CapstoneChatbot.Tmdb.Enums;

namespace CapstoneChatbot.Tmdb.Clients
{
    public class TmdbClient : ITmdbClient
    {
        private readonly HttpClient _httpClient;

        public TmdbClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IReadOnlyList<Models.TmdbSearchResult>> SearchAsync(
            string query,
            MediaType mediaType,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}