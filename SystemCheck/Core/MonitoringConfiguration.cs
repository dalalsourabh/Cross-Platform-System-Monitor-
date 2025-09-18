using SystemCheck.Core.Interfaces;

namespace SystemCheck.Core
{
    /// <summary>
    /// Configuration settings for the monitoring service
    /// </summary>
    public class MonitoringConfiguration : IMonitoringConfiguration
    {
        /// <summary>
        /// Interval in seconds between monitoring updates
        /// </summary>
        public int MonitoringIntervalSeconds { get; set; } = 5;
        
        /// <summary>
        /// API endpoint for sending monitoring data
        /// </summary>
        public string ApiEndpoint { get; set; } = string.Empty;
    }
}