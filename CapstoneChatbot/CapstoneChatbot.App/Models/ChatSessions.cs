namespace CapstoneChatbot.App.Models;

public class ChatSession
{
    public List<string> ConversationHistory { get; } = [];
    public string? LastRecommendationTitle { get; set; }
}
