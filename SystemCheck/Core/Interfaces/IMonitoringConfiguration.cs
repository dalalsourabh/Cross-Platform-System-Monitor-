namespace SystemCheck.Core.Interfaces
{
    public interface IMonitoringConfiguration
    {
        int MonitoringIntervalSeconds { get; set; }
        
        /// API endpoint for sending monitoring data
        string ApiEndpoint { get; set; }
    }
}