using Microsoft.Extensions.Logging;
using SystemCheck.Core.Interfaces;

namespace SystemCheck.Core.Monitoring
{
    /// Windows implementation of disk monitoring
    public class WindowsDiskMonitor : IDiskMonitor
    {
        private readonly ILogger<WindowsDiskMonitor> _logger;
        private readonly string _driveLetter;

        public WindowsDiskMonitor(ILogger<WindowsDiskMonitor> logger)
        {
            _logger = logger;
            _driveLetter = Path.GetPathRoot(Environment.SystemDirectory) ?? "C:\\";
        }

        /// Gets the current used disk space in bytes
        public Task<long> GetUsedDiskSpaceBytesAsync()
        {
            try
            {
                var driveInfo = new DriveInfo(_driveLetter);
                return Task.FromResult(driveInfo.TotalSize - driveInfo.AvailableFreeSpace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting used disk space");
                return Task.FromResult(0L);
            }
        }

        /// Gets the total disk space in bytes
        public Task<long> GetTotalDiskSpaceBytesAsync()
        {
            try
            {
                var driveInfo = new DriveInfo(_driveLetter);
                return Task.FromResult(driveInfo.TotalSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total disk space");
                return Task.FromResult(0L);
            }
        }
    }
}