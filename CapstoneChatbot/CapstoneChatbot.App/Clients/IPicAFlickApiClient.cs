namespace CapstoneChatbot.App.Clients
{
    public interface IPicAFlickApiClient
    {
        Task<List<WatchlistItemDto>> GetWatchlistAsync();
    }
}
