namespace Serilog.Sinks.Loki
{
    public class LokiCredentials
    {
        public LokiCredentials() { }

        public LokiCredentials(string url)
        {
            Url = url;
        }

        public LokiCredentials(string url, string username, string password)
        {
            Url = url;
            Username = username;
            Password = password;
        }

        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}