using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Adeptik.Serilog.Sinks.Loki.ExampleWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog((builder, cfg) =>
                {
                    cfg.ReadFrom.Configuration(builder.Configuration);
                })
                .UseStartup<Startup>()
                .Build();
    }
}
