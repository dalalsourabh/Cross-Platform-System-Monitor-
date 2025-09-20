namespace SystemCheck.Core.Interfaces
{
    public interface ICpuMonitor
    {
        Task<double> GetCpuUsagePercentAsync();
    }
}