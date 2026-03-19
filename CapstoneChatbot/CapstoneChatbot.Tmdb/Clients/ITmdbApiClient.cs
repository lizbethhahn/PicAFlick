using CapstoneChatbot.Tmdb.Models;
using CapstoneChatbot.Tmdb.Enums;

namespace CapstoneChatbot.Tmdb.Clients;

public interface ITmdbApiClient
{
    Task<IReadOnlyList<TmdbSearchResult>> SearchAsync(
        string query,
        MediaType mediaType,
        CancellationToken cancellationToken = default);
    
    Task<TmdbSearchResult?> GetByIdAsync(
        int tmdbId, 
        MediaType mediaType, 
        CancellationToken cancellationToken = default);
}
