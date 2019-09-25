using System.Net.Http;
using System.Threading.Tasks;

namespace Serilog.Sinks.Loki.Example
{
    public class LokiExampleHttpClient : LokiHttpClient
    {
        public override async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) => await base.PostAsync(requestUri, content);
    }
}