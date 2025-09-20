using SystemCheck.Core.Interfaces;

namespace SystemCheck.Core
{
    /// Configuration settings for the monitoring service
    public class MonitoringConfiguration : IMonitoringConfiguration
    {
        public int MonitoringIntervalSeconds { get; set; } = 5;
        
        public string ApiEndpoint { get; set; } = string.Empty;
    }
}