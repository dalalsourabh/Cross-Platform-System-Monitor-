namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring disk usage
    /// </summary>
    public interface IDiskMonitor
    {
        /// <summary>
        /// Gets the current used disk space in bytes
        /// </summary>
        Task<long> GetUsedDiskSpaceBytesAsync();
        
        /// <summary>
        /// Gets the total disk space in bytes
        /// </summary>
        Task<long> GetTotalDiskSpaceBytesAsync();
    }
}