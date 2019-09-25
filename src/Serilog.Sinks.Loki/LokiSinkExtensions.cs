using Serilog.Configuration;
using Serilog.Sinks.Http;
using Serilog.Sinks.Loki.Labels;
using System.Collections.Generic;

namespace Serilog.Sinks.Loki
{
    public static class LokiSinkExtensions
    {
        public static LoggerConfiguration LokiHttp(this LoggerSinkConfiguration sinkConfiguration, LokiCredentials credentials, IEnumerable<LokiLabel> logLabels = null)
            => LokiHttpImpl(sinkConfiguration, credentials, logLabels); 
        
        private static LoggerConfiguration LokiHttpImpl(this LoggerSinkConfiguration sinkConfiguration, LokiCredentials credentials, IEnumerable<LokiLabel> logLabels)
        {
            var formatter = logLabels != null ? new LokiBatchFormatter(logLabels) : new LokiBatchFormatter();
            
            var client = new LokiHttpClient();
            client.SetAuthCredentials(credentials);

            return sinkConfiguration.Http(LokiRouteBuilder.BuildPostUri(credentials.Url), batchFormatter: formatter, httpClient: client);
        }
    }
}