using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SystemCheck.Core.Interfaces;

namespace SystemCheck.Core.Monitoring
{
    /// <summary>
    /// Windows implementation of CPU monitoring using PerformanceCounter
    /// </summary>
    public class WindowsCpuMonitor : ICpuMonitor
    {
        private readonly ILogger<WindowsCpuMonitor> _logger;
        private PerformanceCounter? _cpuCounter;

        public WindowsCpuMonitor(ILogger<WindowsCpuMonitor> logger)
        {
            _logger = logger;
            InitializeCounter();
        }

        private void InitializeCounter()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                // First call to NextValue() always returns 0, so call it once during initialization
                _cpuCounter.NextValue();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize CPU performance counter");
                _cpuCounter = null;
            }
        }

        /// <summary>
        /// Gets the current CPU usage percentage
        /// </summary>
        public Task<double> GetCpuUsagePercentAsync()
        {
            try
            {
                if (_cpuCounter == null)
                {
                    InitializeCounter();
                    if (_cpuCounter == null)
                    {
                        _logger.LogWarning("CPU counter is not available, returning default value");
                        return Task.FromResult(0.0);
                    }
                }

                // Get the current CPU usage
                var cpuUsage = _cpuCounter.NextValue();
                return Task.FromResult((double)cpuUsage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CPU usage");
                return Task.FromResult(0.0);
            }
        }
    }
}