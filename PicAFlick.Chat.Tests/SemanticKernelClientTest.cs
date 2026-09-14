using Microsoft.SemanticKernel;

namespace PicAFlick.Chat.Tests
{
    public class SemanticKernelClientTests
    {
        [Fact]
        public async Task WhenReceivingNullMessage_ShouldThrowArgumentException()
        {
            // Arrange            
            var kernel = Kernel.CreateBuilder().Build();
            var kernelClient = new SemanticKernelClient(kernel);
            var testMessage = null as string;

            // Act 
            var response = await Assert.ThrowsAsync<ArgumentException>(
                async () => await kernelClient.InvokePromptAsync(testMessage)
            );

            // Assert
            Assert.Equal("Message cannot be null or empty. (Parameter 'message')", response.Message);
        }
    }
}