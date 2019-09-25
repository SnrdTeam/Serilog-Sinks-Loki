﻿using System;
using System.Collections.Generic;
using Serilog.Context;
using Serilog.Sinks.Http.BatchFormatters;
using Adeptik.Serilog.Sinks.Loki.Labels;
using Serilog;

namespace Adeptik.Serilog.Sinks.Loki.Example
{
    class Program
    {
        static void Main()
        {            
            Exception ex; 
            try
            {
                throw new Exception("Something went wrong, see StackTrace for more info");
            }
            catch (Exception e)
            {
                ex = e;
            }
            
            var credentials = new LokiCredentials("http://localhost:3100");
            var logLabels = new List<LokiLabel>
            {
                new LokiLabel("app", "demo"),
                new LokiLabel("namespace", "prod")
            };
            var log = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .Enrich.FromLogContext()
                    .WriteTo.LokiHttp(credentials, logLabels, new LokiExampleHttpClient())
                .CreateLogger();

            
            using (LogContext.PushProperty("A", 1))
            {
                
                
            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
/*                log.Information("Carries property A = 1");*/
                log.Information("3# Random message processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            }
            
/*            log.Information("1# Logging {@Heartbeat:l} from {Computer:l}", "SomeValue", "SomeOtherValue");*/

/*            var position = new { Latitude = 25, Longitude = 134 };
            var exception = new {Message = ex.Message, StackTrace = ex.StackTrace};
            var elapsedMs = 34;*/

/*            log.Debug(@"Does this \""break\"" something?");
            log.Error("#2 {@Message}", exception);
            log.Information("3# Random message processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);*/
            
            log.Dispose();
        }
    }
}