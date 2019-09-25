using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog.Events;
using Serilog.Sinks.Loki.Labels;
using Serilog.Sinks.Prometheus;

namespace Serilog.Sinks.Loki.ExampleWebApp
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
