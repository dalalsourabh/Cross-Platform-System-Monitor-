namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring system memory usage
    /// </summary>
    public interface IMemoryMonitor
    {
        /// <summary>
        /// Gets the current used memory in bytes
        /// </summary>
        Task<long> GetUsedMemoryBytesAsync();
        
        /// <summary>
        /// Gets the total system memory in bytes
        /// </summary>
        Task<long> GetTotalMemoryBytesAsync();
    }
}