using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SystemCheck.Core.Interfaces;
using SystemCheck.Core.Models;
using System.Net.Http;
using System.Text;

namespace SystemCheck.Plugins
{
    /// <summary>
    /// Plugin that sends system resource data to a REST API endpoint
    /// </summary>
    public class RestApiPlugin : IMonitorPlugin
    {
        private readonly ILogger<RestApiPlugin> _logger;
        private readonly IMonitoringConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public string Name => "REST API Integration";

        public RestApiPlugin(ILogger<RestApiPlugin> logger, IMonitoringConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Initialize the plugin
        /// </summary>
        public Task InitializeAsync()
        {
            _logger.LogInformation("REST API plugin initialized with endpoint: {ApiEndpoint}", 
                _configuration.ApiEndpoint);
            
            // Add default headers for authentication
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SystemCheck-Monitor/1.0");
            
            // If you need API key authentication, uncomment and configure:
            // _httpClient.DefaultRequestHeaders.Add("X-API-Key", "your-api-key-here");
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle system resource data update by sending to REST API
        /// </summary>
        public async Task HandleUpdateAsync(SystemResourceData data)
        {
            if (string.IsNullOrEmpty(_configuration.ApiEndpoint))
            {
                _logger.LogWarning("API endpoint is not configured. Skipping REST API update.");
                return;
            }

            try
            {
                // Create the payload according to the required format
                var payload = new
                {
                    cpu = data.CpuUsagePercent,
                    ram_used = data.RamUsedBytes,
                    disk_used = data.DiskUsedBytes
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the data to the API endpoint
                var response = await _httpClient.PostAsync(_configuration.ApiEndpoint, content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully sent system resource data to API endpoint");
                }
                else
                {
                    _logger.LogWarning("Failed to send system resource data to API endpoint. Status code: {StatusCode}", 
                        response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending system resource data to API endpoint");
            }
        }
    }
}