namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring disk usage
    /// </summary>
    public interface IDiskMonitor
    {
        Task<long> GetUsedDiskSpaceBytesAsync();

        Task<long> GetTotalDiskSpaceBytesAsync();
    }
}