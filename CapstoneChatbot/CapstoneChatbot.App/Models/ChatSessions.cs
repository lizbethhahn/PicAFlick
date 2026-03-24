using CapstoneChatbot.Tmdb.Models;

namespace CapstoneChatbot.App.Models;

public class ChatSession
{
    public List<string> ConversationHistory { get; } = [];
    public string? LastRecommendationTitle { get; set; }
    public List<TmdbSearchResult> PendingAddResults { get; } = [];
    public bool IsWaitingForAddSelection { get; set; }
}
