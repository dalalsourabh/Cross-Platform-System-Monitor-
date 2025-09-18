namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring configuration settings
    /// </summary>
    public interface IMonitoringConfiguration
    {
        /// <summary>
        /// Interval in seconds between monitoring updates
        /// </summary>
        int MonitoringIntervalSeconds { get; set; }
        
        /// <summary>
        /// API endpoint for sending monitoring data
        /// </summary>
        string ApiEndpoint { get; set; }
    }
}