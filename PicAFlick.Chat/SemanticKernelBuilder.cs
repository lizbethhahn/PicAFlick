using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace PicAFlick.Chat
{
    public class SemanticKernelBuilder
    {
        private readonly IConfiguration _configuration;
        public SemanticKernelBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Kernel Build()
        {
            var deploymentName = _configuration["AzureOpenAI:DeploymentName"]
                ?? throw new InvalidOperationException("AzureOpenAI:DeploymentName user secret is missing.");
            var endpoint = _configuration["AzureOpenAI:Endpoint"]
                ?? throw new InvalidOperationException("AzureOpenAI:Endpoint user secret is missing.");
            var apiKey = _configuration["AzureOpenAI:ApiKey"]
                ?? throw new InvalidOperationException("AzureOpenAI:ApiKey user secret is missing.");

            var kernelBuilder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(
                    deploymentName: deploymentName,
                    endpoint: endpoint,
                    apiKey: apiKey
                );

            var kernel = kernelBuilder.Build();
            return kernel;
        }
    }
}
