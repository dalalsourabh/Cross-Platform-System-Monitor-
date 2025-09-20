using Microsoft.Extensions.Logging;
using SystemCheck.Core.Interfaces;
using SystemCheck.Core.Models;

namespace SystemCheck.Core.Monitoring
{
    /// Main service that coordinates system resource monitoring
    public class MonitoringService : IMonitoringService
    {
        private readonly ICpuMonitor _cpuMonitor;
        private readonly IMemoryMonitor _memoryMonitor;
        private readonly IDiskMonitor _diskMonitor;
        private readonly IEnumerable<IMonitorPlugin> _plugins;
        private readonly IMonitoringConfiguration _configuration;
        private readonly ILogger<MonitoringService> _logger;
        
        private Timer? _timer;
        private bool _isRunning;

        public MonitoringService(
            ICpuMonitor cpuMonitor,
            IMemoryMonitor memoryMonitor,
            IDiskMonitor diskMonitor,
            IEnumerable<IMonitorPlugin> plugins,
            IMonitoringConfiguration configuration,
            ILogger<MonitoringService> logger)
        {
            _cpuMonitor = cpuMonitor;
            _memoryMonitor = memoryMonitor;
            _diskMonitor = diskMonitor;
            _plugins = plugins;
            _configuration = configuration;
            _logger = logger;
        }

        public void Start()
        {
            if (_isRunning)
            {
                _logger.LogWarning("Monitoring service is already running");
                return;
            }

            _logger.LogInformation("Starting monitoring service with interval of {Interval} seconds", 
                _configuration.MonitoringIntervalSeconds);
            
            // Initialize all plugins
            InitializePlugins();
            
            // Start the timer to collect system resource data
            var interval = TimeSpan.FromSeconds(_configuration.MonitoringIntervalSeconds);
            _timer = new Timer(CollectAndProcessData, null, TimeSpan.Zero, interval);
            _isRunning = true;
        }
        
        public void Stop()
        {
            if (!_isRunning)
            {
                _logger.LogWarning("Monitoring service is not running");
                return;
            }

            _logger.LogInformation("Stopping monitoring service");
            _timer?.Dispose();
            _timer = null;
            _isRunning = false;
        }

        private async void InitializePlugins()
        {
            foreach (var plugin in _plugins)
            {
                try
                {
                    _logger.LogInformation("Initializing plugin: {PluginName}", plugin.Name);
                    await plugin.InitializeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to initialize plugin: {PluginName}", plugin.Name);
                }
            }
        }

        private async void CollectAndProcessData(object? state)
        {
            try
            {
                var data = await CollectSystemResourceDataAsync();
                
                // Display data in console
                DisplaySystemResourceData(data);
                
                // Process data with all plugins
                await ProcessDataWithPluginsAsync(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error collecting or processing system resource data");
            }
        }

        private async Task<SystemResourceData> CollectSystemResourceDataAsync()
        {
            var cpuTask = _cpuMonitor.GetCpuUsagePercentAsync();
            var ramUsedTask = _memoryMonitor.GetUsedMemoryBytesAsync();
            var ramTotalTask = _memoryMonitor.GetTotalMemoryBytesAsync();
            var diskUsedTask = _diskMonitor.GetUsedDiskSpaceBytesAsync();
            var diskTotalTask = _diskMonitor.GetTotalDiskSpaceBytesAsync();

            await Task.WhenAll(cpuTask, ramUsedTask, ramTotalTask, diskUsedTask, diskTotalTask);

            return new SystemResourceData
            {
                CpuUsagePercent = await cpuTask,
                RamUsedBytes = await ramUsedTask,
                RamTotalBytes = await ramTotalTask,
                DiskUsedBytes = await diskUsedTask,
                DiskTotalBytes = await diskTotalTask,
                Timestamp = DateTime.UtcNow
            };
        }

        private void DisplaySystemResourceData(SystemResourceData data)
        {
            Console.Clear();
            Console.WriteLine("=== System Resource Monitor ===");
            Console.WriteLine($"Time: {data.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"CPU Usage: {data.CpuUsagePercent:F2}%");
            Console.WriteLine($"RAM Usage: {FormatBytes(data.RamUsedBytes)}/{FormatBytes(data.RamTotalBytes)} ({data.RamUsagePercent:F2}%)");
            Console.WriteLine($"Disk Usage: {FormatBytes(data.DiskUsedBytes)}/{FormatBytes(data.DiskTotalBytes)} ({data.DiskUsagePercent:F2}%)");
            Console.WriteLine("===============================");
        }

        private async Task ProcessDataWithPluginsAsync(SystemResourceData data)
        {
            foreach (var plugin in _plugins)
            {
                try
                {
                    await plugin.HandleUpdateAsync(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing data with plugin: {PluginName}", plugin.Name);
                }
            }
        }

        private string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int counter = 0;
            decimal number = bytes;
            
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            
            return $"{number:F2} {suffixes[counter]}";
        }
    }
}