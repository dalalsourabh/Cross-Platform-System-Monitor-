namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for the main monitoring service
    /// </summary>
    public interface IMonitoringService
    {
        /// <summary>
        /// Start the monitoring service
        /// </summary>
        void Start();
        
        /// <summary>
        /// Stop the monitoring service
        /// </summary>
        void Stop();
    }
}