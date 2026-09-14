using Microsoft.SemanticKernel;

namespace PicAFlick.Chat
{
    public class SemanticKernelChatResponder : IChatResponder
    {
        private readonly ISemanticKernelClient _kernelClient;

        public SemanticKernelChatResponder(ISemanticKernelClient kernelClient)
        {
            _kernelClient = kernelClient;
        }

        public async Task<string?> GenerateResponseAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException();

            return await _kernelClient.InvokePromptAsync(message);
        }
    }
}