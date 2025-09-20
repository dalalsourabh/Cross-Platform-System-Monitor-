using SystemCheck.Core.Models;

namespace SystemCheck.Core.Interfaces
{
    /// Interface for monitoring plugins that can respond to system resource updates
    public interface IMonitorPlugin
    {
        /// Name of the plugin
        string Name { get; }
        
        /// Called when new system resource data is available
        /// <param name="data">The current system resource data</param>
        Task HandleUpdateAsync(SystemResourceData data);
        
        /// Initialize the plugin with any required setup
        Task InitializeAsync();
    }
}