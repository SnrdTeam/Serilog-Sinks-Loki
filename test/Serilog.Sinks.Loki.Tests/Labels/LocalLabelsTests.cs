using Adeptik.Serilog.Sinks.Loki.Tests.Infrastructure;
using Xunit;

namespace Adeptik.Serilog.Sinks.Loki.Tests.Labels
{
    public class LocalLabelsTests : IClassFixture<HttpClientTestFixture>
    {
        private readonly HttpClientTestFixture _httpClientTestFixture;
        private readonly TestHttpClient _client;

        public LocalLabelsTests(HttpClientTestFixture httpClientTestFixture)
        {
            _httpClientTestFixture = httpClientTestFixture;
            _client = new TestHttpClient();
        }
    }
}