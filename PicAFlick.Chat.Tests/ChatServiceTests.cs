using Moq;

namespace PicAFlick.Chat.Tests
{
    public class ChatServiceTests
    {
        [Fact]
        public async Task WhenBlankMessageIsReceived_ShouldThrowArgumentException()
        {
            //Arrange
            var responder = new Mock<IChatResponder>();
            var service = new ChatService(responder.Object);
            var message = " ";

            //Act & Assert
            var result = await Assert.ThrowsAsync<ArgumentException>(
                async () => await service.ProcessIncomingMessageAsync(message)
            );            
        }

        [Fact]
        public async Task WhenValidMessageIsReceived_ShouldNotThrowArgumentException()
        {
            //Arrange
            var responder = new Mock<IChatResponder>();
            var service = new ChatService(responder.Object);
            var message = "Tell me about Alien.";

            //Act
            var exception = await Record.ExceptionAsync(
                async () => await service.ProcessIncomingMessageAsync(message)
            );

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task WhenValidMessageIsReceived_ShouldReturnDifferentResponse()
        {
            //Arrange           
            var responder = new Mock<IChatResponder>();
            var service = new ChatService(responder.Object);
            var message = "Tell me about Dune.";

            //Act
            var result = await service.ProcessIncomingMessageAsync(message);

            //Assert
            Assert.NotEqual(message,result);
        }

        [Fact]
        public async Task WhenProcessIncomingMessageIsCalled_ShouldReturnBotResponse()
        {
            //Arrange
            var responder = new Mock<IChatResponder>();     
            var service = new ChatService(responder.Object);
            var message = "Tell me about Dune";
            var botResponse = "Dune is...";

            responder.Setup(r => r.GenerateResponseAsync(message)).ReturnsAsync(botResponse);

            //Act
            var result = await service.ProcessIncomingMessageAsync(message);

            //Assert
            Assert.Equal(botResponse, result);

        }
    }
}