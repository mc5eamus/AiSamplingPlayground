using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AiSamplingPlayground
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<TelemetryConfiguration>((config) =>
                    {
                        var builder = config.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
                        builder.UseSampling(99);
                        builder.Build();
                    });

                    services.AddApplicationInsightsTelemetryWorkerService();

                    services.AddSingleton<ITelemetryInitializer, SampleTelemetryInitializer>();
                    services.AddHostedService<MonitoringEventEmitter>();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                // Start the host
                await host.StartAsync();

                // Wait for the host to shutdown
                await host.WaitForShutdownAsync();
            }
        }
    }
}
