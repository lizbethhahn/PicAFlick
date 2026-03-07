using CapstoneChatbot.Tmdb.Enums;

namespace CapstoneChatbot.Tmdb.Clients
{
    public class TmdbClient : ITmdbClient
    {
        public Task<IReadOnlyList<Models.TmdbSearchResult>> SearchAsync(
            string query,
            MediaType mediaType,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}