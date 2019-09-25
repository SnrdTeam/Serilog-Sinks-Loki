# Adeptik.Serilog.Sinks.Loki

This is a Serilog Sink for Grafana's new [Loki Log Aggregator](https://grafana.com/loki).

What is Loki?

> Loki is a horizontally-scalable, highly-available, multi-tenant log aggregation system inspired by Prometheus. It is designed to be very cost effective and easy to operate, as it does not index the contents of the logs, but rather a set of labels for each log stream.

You can find more information about what Loki is over on [Grafana's website here](https://grafana.com/loki).

![Loki Screenshot](https://raw.githubusercontent.com/JosephWoodward/Serilog-Sinks-Loki/master/assets/screenshot2.png)

## Current Features:

- Formats and batches log entries to Loki via HTTP
- Ability to provide global Loki log labels
- Comes baked with an HTTP client, but your own can be provided
- Works witn [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) package

Coming soon:

- Ability to provide contextual log labels
- Write logs to disk in the correct format to send via Promtail
- Send logs to Loki via HTTP using Snappy compression

## Installation

The Adeptik.Serilog.Sinks.Loki NuGet [package can be found here](https://www.nuget.org/packages/Adeptik.Serilog.Sinks.Loki/). Alternatively you can install it via one of the following commands below:

NuGet command:
```bash
Install-Package Adeptik.Serilog.Sinks.Loki
```
.NET Core CLI:
```bash
dotnet add package Adeptik.Serilog.Sinks.Loki
```

## Basic Example:

```csharp
var credentials = new LokiCredentials("http://localhost:3100"); // Address to local or remote Loki server with no auth
// or var credentials = new LokiCredentials("http://localhost:3100", "login", "pass"); for basic auth

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.LokiHttp(credentials)
        .CreateLogger();

var exception = new {Message = ex.Message, StackTrace = ex.StackTrace};
Log.Error(exception);

var position = new { Latitude = 25, Longitude = 134 };
var elapsedMs = 34;
Log.Information("Message processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

Log.CloseAndFlush();
```

### Adding global labels

Loki indexes and groups log streams using labels, in Adeptik.Serilog.Sinks.Loki you can attach labels to all log entries by passing an list of labels to the `WriteTo.LokiHttp(..)` configuration method. This is ideal for labels such as instance IDs, environments and application names:

```csharp
var labels = new List<LokiLabel>
{
    new LokiLabel("app", "demo"),
    new LokiLabel("namespace", "prod")
};

var credentials = new LokiCredentials("http://localhost:3100");

var logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.LokiHttp(credentials, labels)
        .CreateLogger();
```

### Local, contextual labels

In some occasions you'll want to add labels to your log stream within a particular class or method, this feature isn't quite finished yet but will be available soon.

### Custom HTTP Client

Adeptik.Serilog.Loki.Sink is built on top of the popular [Serilog.Sinks.Http](https://github.com/FantasticFiasco/serilog-sinks-http) library to post log entries to Loki. With this in mind you can you can extend the default HttpClient (`LokiHttpClient`), or replace it entirely by implementing `IHttpClient`.

```csharp
// ExampleHttpClient.cs

public class ExampleHttpClient : LokiHttpClient
{
    public override Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        return base.PostAsync(requestUri, content);
    }
}
```
```csharp
// Usage

var credentials = new BasicAuthCredentials("http://localhost:3100", "<username>", "<password>");
var logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.LokiHttp(credentials, labels, new ExampleHttpClient())
        .CreateLogger();
```

### JSON `appsettings.json` configuration

To use the console sink with _Microsoft.Extensions.Configuration_, for example with ASP.NET Core or .NET Core, use the [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) package.

Instead of configuring the sink directly in code, call `ReadFrom.Configuration()`:

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
```

In your `appsettings.json` file, under the `Serilog` node, :
```json
{
  "Serilog": {
    "WriteTo": [
        {
            "Name": "LokiHttp",
            "Args": {
                "credentials": {
                    "Url": "http://192.168.43.30:3100",
                    "Username": "login",
                    "Password": "pass"
                },
                "logLabels": [
                    {
                        "Key": "app",
                        "Value": "demo"
                    },
                    {
                        "Key": "namespace",
                        "Value": "production"
                    }
                ]
            }
        }
    ]
  }
}
```

### Missing a feature or want to contribute?
This package is still in its infancy so if there's anything missing then please feel free to raise a feature request, either that or pull requests are most welcome!
