namespace Serilog.Sinks.Loki.Labels
{
    public class LokiLabel
    {
        public LokiLabel() { }

        public LokiLabel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        internal string GetString() => $"{Key}=\\\"{Value}\\\"";
    }
}