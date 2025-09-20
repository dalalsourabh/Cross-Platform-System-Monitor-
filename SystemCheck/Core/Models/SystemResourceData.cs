namespace SystemCheck.Core.Models
{
    /// Represents system resource data collected during monitoring
    public class SystemResourceData
    {
        public double CpuUsagePercent { get; set; }
        
        /// Used RAM in bytes
        public long RamUsedBytes { get; set; }
        
        public long RamTotalBytes { get; set; }
        
        public long DiskUsedBytes { get; set; }
        
        public long DiskTotalBytes { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        public double RamUsagePercent => RamTotalBytes > 0 ? (double)RamUsedBytes / RamTotalBytes * 100 : 0;
        
        /// Disk usage percentage (0-100)
        public double DiskUsagePercent => DiskTotalBytes > 0 ? (double)DiskUsedBytes / DiskTotalBytes * 100 : 0;
    }
}