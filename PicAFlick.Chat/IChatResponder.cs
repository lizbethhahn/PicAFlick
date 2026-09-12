namespace PicAFlick.Chat
{
    public interface IChatResponder
    {
        Task<string> GenerateResponseAsync(string message);
    }
}
