using SystemCheck.Core.Models;

namespace SystemCheck.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring plugins that can respond to system resource updates
    /// </summary>
    public interface IMonitorPlugin
    {
        /// <summary>
        /// Name of the plugin
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Called when new system resource data is available
        /// </summary>
        /// <param name="data">The current system resource data</param>
        Task HandleUpdateAsync(SystemResourceData data);
        
        /// <summary>
        /// Initialize the plugin with any required setup
        /// </summary>
        Task InitializeAsync();
    }
}