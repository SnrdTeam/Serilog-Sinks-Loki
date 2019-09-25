using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Adeptik.Serilog.Sinks.Loki.Labels;
using Serilog.Sinks.Loki.Tests.Infrastructure;
using Shouldly;
using Xunit;
using Adeptik.Serilog.Sinks.Loki;

namespace Serilog.Sinks.Loki.Tests.Labels
{
    public class GlobalLabelsTests : IClassFixture<HttpClientTestFixture>
    {
        private readonly TestHttpClient _client;
        private readonly LokiCredentials _credentials;

        public GlobalLabelsTests()
        {
            _client = new TestHttpClient();
            _credentials = new LokiCredentials("http://test:80", "Walter", "White");
        }
        
        [Fact]
        public void GlobalLabelsCanBeSet()
        {
            var logLabels = new List<LokiLabel>
            {
                new LokiLabel("app", "demo"),
                new LokiLabel("namespace", "prod")
            };

            // Arrange
            var log = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.LokiHttp(_credentials, logLabels, _client)
                .CreateLogger();
            
            // Act
            log.Error("Something's wrong");
            log.Dispose();
            
            // Assert
            var response = JsonConvert.DeserializeObject<TestResponse>(_client.Content);
            response.Streams.First().Labels.ShouldBe("{app=\"demo\",namespace=\"prod\",level=\"error\"}");
        }
    }
}