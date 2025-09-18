using Microsoft.Extensions.Logging;
using SystemCheck.Core.Interfaces;

namespace SystemCheck.Core.Monitoring
{
    /// <summary>
    /// Windows implementation of disk monitoring
    /// </summary>
    public class WindowsDiskMonitor : IDiskMonitor
    {
        private readonly ILogger<WindowsDiskMonitor> _logger;
        private readonly string _driveLetter;

        public WindowsDiskMonitor(ILogger<WindowsDiskMonitor> logger)
        {
            _logger = logger;
            _driveLetter = Path.GetPathRoot(Environment.SystemDirectory) ?? "C:\\";
        }

        /// <summary>
        /// Gets the current used disk space in bytes
        /// </summary>
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

        /// <summary>
        /// Gets the total disk space in bytes
        /// </summary>
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