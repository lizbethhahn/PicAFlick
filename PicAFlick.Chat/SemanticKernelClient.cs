using Microsoft.SemanticKernel;

namespace PicAFlick.Chat
{
    public class SemanticKernelClient : ISemanticKernelClient
    {   
        private readonly Kernel _kernel;

        public SemanticKernelClient(Kernel kernel) 
        {
            _kernel = kernel;
        }  
        
        public async Task<string> InvokePromptAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
           
            var result = await _kernel.InvokePromptAsync(message);

            return result.GetValue<string>();
        }
    }
}