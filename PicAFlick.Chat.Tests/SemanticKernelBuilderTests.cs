using Microsoft.Extensions.Configuration;

namespace PicAFlick.Chat.Tests
{
    public class SemanticKernelBuilderTests
    {
        [Theory]
        [InlineData("AzureOpenAI:DeploymentName")]
        [InlineData("AzureOpenAI:Endpoint")]
        [InlineData("AzureOpenAI:ApiKey")]
        public void WhenRequiredAzureOpenAiConfigIsMissing_ShouldThrowInvalidOperationException(string missingConfigKey)
        {
            // Arrange
            var configValues = new Dictionary<string, string>
            {
                { "AzureOpenAI:DeploymentName", "test-deployment" },
                { "AzureOpenAI:Endpoint", "https://example.com" },
                { "AzureOpenAI:ApiKey", "test-api-key" }
            };

            configValues.Remove(missingConfigKey);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            var builder = new SemanticKernelBuilder(configuration);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void WhenAllRequiredAzureOpenAiConfigsArePresent_ShouldSuccessfullyCreateKernel()
        {
            // Arrange
            var configValues = new Dictionary<string, string>
            {
                { "AzureOpenAI:DeploymentName", "test-deployment" },
                { "AzureOpenAI:Endpoint", "https://example.com" },
                { "AzureOpenAI:ApiKey", "test-api-key" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();
            var builder = new SemanticKernelBuilder(configuration);

            // Act
            var result = builder.Build();

            //Assert
            Assert.NotNull(result);
        }
    }
}