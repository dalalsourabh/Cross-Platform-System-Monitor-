using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using SystemCheck.Core.Interfaces;

namespace SystemCheck.Core.Monitoring
{
    /// Windows implementation of memory monitoring
    public class WindowsMemoryMonitor : IMemoryMonitor
    {
        private readonly ILogger<WindowsMemoryMonitor> _logger;

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        public WindowsMemoryMonitor(ILogger<WindowsMemoryMonitor> logger)
        {
            _logger = logger;
        }

        /// Gets the current used memory in bytes
        public Task<long> GetUsedMemoryBytesAsync()
        {
            try
            {
                var memoryStatus = GetMemoryStatus();
                return Task.FromResult((long)(memoryStatus.ullTotalPhys - memoryStatus.ullAvailPhys));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting used memory");
                return Task.FromResult(0L);
            }
        }

        /// Gets the total system memory in bytes
        public Task<long> GetTotalMemoryBytesAsync()
        {
            try
            {
                var memoryStatus = GetMemoryStatus();
                return Task.FromResult((long)memoryStatus.ullTotalPhys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total memory");
                return Task.FromResult(0L);
            }
        }

        private MEMORYSTATUSEX GetMemoryStatus()
        {
            var memoryStatus = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX)) };
            if (!GlobalMemoryStatusEx(ref memoryStatus))
            {
                throw new Exception("Failed to get memory status: " + Marshal.GetLastWin32Error());
            }
            return memoryStatus;
        }
    }
}