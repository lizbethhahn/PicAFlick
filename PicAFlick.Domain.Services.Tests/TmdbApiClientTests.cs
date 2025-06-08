using Moq;
using Moq.Protected;
using PicAFlick.Data;

namespace PicAFlick.Domain.Services.Tests
{
    public class TmdbApiClientTests
    {
        [Fact]
        public async Task GetMovieByTitleAsync_ShouldReturnMovieInfo()
        {
            // arrange
            Environment.SetEnvironmentVariable("TMDB_API_KEY", "FAKE_API_KEY");
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"page\":1,\"results\":[{\"title\":\"Inception\",\"id\":27205}],\"total_pages\":1,\"total_results\":1}")
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.themoviedb.org/3/"),
                DefaultRequestHeaders = { { "Accept", "application/json" } }
            };

            var tmdbApiClient = new TmdbApiClient(httpClient);

            // act
            var result = await tmdbApiClient.GetMovieByTitleAsync("Inception");

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Results);
            Assert.Equal("Inception", result.Results.First().Title);
        }

        [Fact]
        public async Task GetTvShowByTitle_ShouldReturnTvShowInfo()
        {
            // arrange
            Environment.SetEnvironmentVariable("TMDB_API_KEY", "FAKE_API_KEY");
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"page\":1,\"results\":[{\"name\":\"Breaking Bad\",\"id\":169}],\"total_pages\":1,\"total_results\":1}")
                });
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.themoviedb.org/3/"),
                DefaultRequestHeaders = { { "Accept", "application/json" } }
            };
            var tmdbApiClient = new TmdbApiClient(httpClient);
            // act
            var result = await tmdbApiClient.GetTvShowByTitleAsync("Breaking Bad");
            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Results);
            Assert.Equal("Breaking Bad", result.Results.First().Title);
        }
    }
}