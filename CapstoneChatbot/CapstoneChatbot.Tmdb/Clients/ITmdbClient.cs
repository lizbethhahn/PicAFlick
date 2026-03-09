using CapstoneChatbot.Tmdb.Models;
using CapstoneChatbot.Tmdb.Enums;

namespace CapstoneChatbot.Tmdb.Clients;

public interface ITmdbClient
{
    Task<IReadOnlyList<TmdbSearchResult>> SearchAsync(
        string query,
        MediaType mediaType,
        CancellationToken cancellationToken = default);
}
