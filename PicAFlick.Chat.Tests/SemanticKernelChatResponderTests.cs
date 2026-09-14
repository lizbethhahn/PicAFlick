using Microsoft.SemanticKernel;
using Moq;

namespace PicAFlick.Chat.Tests
{
    public class SemanticKernelChatResponderTests
    {
        [Fact]
        public async Task WhenGenerateResponseAsyncIsCalledWithBlankMessage_ShouldThrowArgumentException()
        {
            // Arrange
            var kernelClient = new Mock<ISemanticKernelClient>();
            var responder = new SemanticKernelChatResponder(kernelClient.Object); 

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => responder.GenerateResponseAsync(string.Empty));
        }

        [Fact]
        public async Task WhenGenerateResponseAsyncIsCalledWithValidMessage_ShouldReturnResponse()
        {
            // Arrange
            var message = "Tell me about Dune.";
            var expectedResponse = "Dune is a science fiction novel by Frank Herbert.";

            var kernelClient = new Mock<ISemanticKernelClient>();

            kernelClient
                .Setup(client => client.InvokePromptAsync(message))
                .ReturnsAsync(expectedResponse);

            var responder = new SemanticKernelChatResponder(kernelClient.Object);

            // Act
            var response = await responder.GenerateResponseAsync(message);

            // Assert
            Assert.Equal(expectedResponse, response);
        }
    }
}