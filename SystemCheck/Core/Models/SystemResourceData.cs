namespace SystemCheck.Core.Models
{
    /// <summary>
    /// Represents system resource data collected during monitoring
    /// </summary>
    public class SystemResourceData
    {
        /// <summary>
        /// CPU usage percentage (0-100)
        /// </summary>
        public double CpuUsagePercent { get; set; }
        
        /// <summary>
        /// Used RAM in bytes
        /// </summary>
        public long RamUsedBytes { get; set; }
        
        /// <summary>
        /// Total RAM in bytes
        /// </summary>
        public long RamTotalBytes { get; set; }
        
        /// <summary>
        /// Used disk space in bytes
        /// </summary>
        public long DiskUsedBytes { get; set; }
        
        /// <summary>
        /// Total disk space in bytes
        /// </summary>
        public long DiskTotalBytes { get; set; }
        
        /// <summary>
        /// Timestamp when the data was collected
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// RAM usage percentage (0-100)
        /// </summary>
        public double RamUsagePercent => RamTotalBytes > 0 ? (double)RamUsedBytes / RamTotalBytes * 100 : 0;
        
        /// <summary>
        /// Disk usage percentage (0-100)
        /// </summary>
        public double DiskUsagePercent => DiskTotalBytes > 0 ? (double)DiskUsedBytes / DiskTotalBytes * 100 : 0;
    }
}