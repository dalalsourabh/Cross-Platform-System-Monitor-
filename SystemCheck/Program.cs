using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemCheck.Core;
using SystemCheck.Core.Interfaces;
using SystemCheck.Core.Monitoring;
using SystemCheck.Plugins;

namespace SystemCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var monitoringService = host.Services.GetRequiredService<IMonitoringService>();
            
            // Start the monitoring service
            monitoringService.Start();
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            
            monitoringService.Stop();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Register core services
                    services.AddSingleton<IMonitoringService, MonitoringService>();
                    
                    // Register platform-specific resource monitors
                    services.AddSingleton<ICpuMonitor, WindowsCpuMonitor>();
                    services.AddSingleton<IMemoryMonitor, WindowsMemoryMonitor>();
                    services.AddSingleton<IDiskMonitor, WindowsDiskMonitor>();
                    
                    // Register plugins
                    services.AddSingleton<IMonitorPlugin, FileLoggerPlugin>();
                    services.AddSingleton<IMonitorPlugin, RestApiPlugin>();
                    
                    // Register configuration
                    services.AddSingleton<IMonitoringConfiguration>(sp =>
                    {
                        var config = sp.GetRequiredService<IConfiguration>();
                        return new MonitoringConfiguration
                        {
                            MonitoringIntervalSeconds = config.GetValue<int>("MonitoringIntervalSeconds"),
                            ApiEndpoint = config.GetValue<string>("ApiEndpoint")
                        };
                    });
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConsole();
                });
    }
}