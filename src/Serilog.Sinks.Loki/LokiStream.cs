using Newtonsoft.Json;
using Serilog.Sinks.Loki.Labels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Sinks.Loki
{
    internal class LokiStream
    {
        private IEnumerable<LokiLabel> _labels { get; set; }

        private IEnumerable<LokiEntry> _entries { get; set; }

        internal LokiStream()
        {
            _labels = new List<LokiLabel>();
            _entries = new List<LokiEntry>();
        }

        internal LokiStream(IEnumerable<LokiLabel> labels, IEnumerable<LokiEntry> entries)
        {
            _labels = labels ?? throw new ArgumentNullException(nameof(labels));
            _entries = entries ?? throw new ArgumentNullException(nameof(entries));
        }

        internal string GetString()
            => $"{{\"labels\":\"{{{string.Join(",", _labels.Select(x => x.GetString()))}}}\"," +
               $"\"entries\":[{string.Join(",", _entries.Select(x => JsonConvert.SerializeObject(x)))}]}}";
    }
}
