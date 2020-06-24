using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiSamplingPlayground
{
    class MonitoringEventEmitter : BackgroundService, IHostedService, IDisposable
    {
        ILogger logger;
        TelemetryClient telemetryClient;
        int counter = 0;

        public MonitoringEventEmitter(ILogger<MonitoringEventEmitter> logger, TelemetryClient telemetryClient)
        {
            this.logger = logger;
            this.telemetryClient = telemetryClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                stoppingToken.ThrowIfCancellationRequested();
                telemetryClient.TrackTrace($"Trace {++counter} emitted;");                
                await Task.Delay(100);
            }
        }
    }
}
