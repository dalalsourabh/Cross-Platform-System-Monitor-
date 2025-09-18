namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring CPU usage
    /// </summary>
    public interface ICpuMonitor
    {
        /// <summary>
        /// Gets the current CPU usage percentage
        /// </summary>
        /// <returns>CPU usage as a percentage (0-100)</returns>
        Task<double> GetCpuUsagePercentAsync();
    }
}