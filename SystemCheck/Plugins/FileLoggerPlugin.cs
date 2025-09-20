using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SystemCheck.Core.Interfaces;
using SystemCheck.Core.Models;

namespace SystemCheck.Plugins
{
    /// Plugin that logs system resource data to a file
    public class FileLoggerPlugin : IMonitorPlugin
    {
        private readonly ILogger<FileLoggerPlugin> _logger;
        private readonly IConfiguration _configuration;
        private string _logFilePath;

        public string Name => "File Logger";

        public FileLoggerPlugin(ILogger<FileLoggerPlugin> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _logFilePath = _configuration["FileLogger:FilePath"] ?? "logs/system_stats.log";
        }

        /// Initialize the plugin by ensuring the log directory exists
        public Task InitializeAsync()
        {
            try
            {
                // Ensure the directory exists
                var directory = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                _logger.LogInformation("File logger initialized with log file: {LogFilePath}", _logFilePath);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize file logger");
                return Task.CompletedTask;
            }
        }

        /// Handle system resource data update by logging to file
        public Task HandleUpdateAsync(SystemResourceData data)
        {
            try
            {
                var logEntry = new
                {
                    Timestamp = data.Timestamp,
                    CpuUsage = data.CpuUsagePercent,
                    RamUsed = data.RamUsedBytes,
                    RamTotal = data.RamTotalBytes,
                    RamUsagePercent = data.RamUsagePercent,
                    DiskUsed = data.DiskUsedBytes,
                    DiskTotal = data.DiskTotalBytes,
                    DiskUsagePercent = data.DiskUsagePercent
                };

                var json = JsonConvert.SerializeObject(logEntry);
                File.AppendAllText(_logFilePath, json + Environment.NewLine);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log system resource data to file");
                return Task.CompletedTask;
            }
        }
    }
}