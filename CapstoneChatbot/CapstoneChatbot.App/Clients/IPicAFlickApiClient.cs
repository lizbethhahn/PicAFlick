using PicAFlick.Shared.Contracts;

namespace CapstoneChatbot.App.Clients
{
    public interface IPicAFlickApiClient
    {
        Task<List<WatchlistItemDto>> GetWatchlistAsync();
        Task AddToWatchlistAsync(WatchlistCreationDto item);
        Task MarkAsWatchedAsync(int id);
        Task RemoveFromWatchlistAsync(int id);
    }
}
