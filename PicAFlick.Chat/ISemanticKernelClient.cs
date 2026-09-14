namespace PicAFlick.Chat
{
    public interface ISemanticKernelClient
    {
        Task<string> InvokePromptAsync(string message);
    }
}