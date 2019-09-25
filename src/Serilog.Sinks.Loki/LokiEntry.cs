using Newtonsoft.Json;

namespace Adeptik.Serilog.Sinks.Loki
{
    internal class LokiEntry 
    {
        [JsonProperty("ts")]
        public string Ts { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }
    }
}