using System;

namespace SystemCheckAPI.Models
{
    public class SystemResourceData
    {
        public DateTime Timestamp { get; set; }
        public double CpuUsage { get; set; }
        public long RamUsed { get; set; }
        public long RamTotal { get; set; }
        public double RamUsagePercent { get; set; }
        public long DiskUsed { get; set; }
        public long DiskTotal { get; set; }
        public double DiskUsagePercent { get; set; }
    }
}