namespace SystemCheck.Core.Interfaces
{
    public interface IMemoryMonitor
    {
        Task<long> GetUsedMemoryBytesAsync();
        
        Task<long> GetTotalMemoryBytesAsync();
    }
}