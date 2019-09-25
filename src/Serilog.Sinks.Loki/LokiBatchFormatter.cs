using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Http;
using Serilog.Sinks.Loki.Labels;

namespace Serilog.Sinks.Loki
{
    internal class LokiBatchFormatter : IBatchFormatter 
    {
        private readonly IEnumerable<LokiLabel> _globalLabels;

        public LokiBatchFormatter()
        {
            _globalLabels = new List<LokiLabel>();
        }

        public LokiBatchFormatter(IEnumerable<LokiLabel> globalLabels)
        {
            _globalLabels = globalLabels;
        }
        
        public void Format(IEnumerable<LogEvent> logEvents, ITextFormatter formatter, TextWriter output)
        {
            if (logEvents == null)
                throw new ArgumentNullException(nameof(logEvents));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            if (!logEvents.Any())
                return;

            var streams = logEvents.Select(x => new LokiStream(_globalLabels.Concat(new List<LokiLabel> { new LokiLabel("level", x.Level.ToString().ToLower()) }).ToList(),
                    new List<LokiEntry> { new LokiEntry { Ts = x.Timestamp.ToString("o"), Line = $"{x.RenderMessage()}{GetExceptionDetails(x.Exception)}" } })).ToList();

            output.Write(LokiStreamsToString(streams));
        }

        public void Format(IEnumerable<string> logEvents, TextWriter output)
        {
            throw new NotImplementedException();
        }

        private static string GetExceptionDetails(Exception ex)
        {
            if (ex == null)
                return "";

            return "\n" + ex.ToStringDemystified();
        }

        private string LokiStreamsToString(IEnumerable<LokiStream> streams)
            => $"{{\"streams\":[{string.Join(",", streams.Select(x => x.GetString()))}]}}";
    }
}