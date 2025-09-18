using Microsoft.AspNetCore.Mvc;
using SystemCheckAPI.Models;
using SystemCheckAPI.Services;

namespace SystemCheckAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemDataController : ControllerBase
    {
        private readonly SystemDataService _dataService;
        private readonly ILogger<SystemDataController> _logger;

        public SystemDataController(SystemDataService dataService, ILogger<SystemDataController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PostSystemData([FromBody] SystemResourceData data)
        {
            if (data == null)
            {
                return BadRequest("Data cannot be null");
            }

            _logger.LogInformation("Received system data: CPU {CpuUsage}%, RAM {RamUsage}%", 
                data.CpuUsage, data.RamUsagePercent);
            
            _dataService.AddData(data);
            return Ok(new { message = "Data received successfully" });
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var data = _dataService.GetAllData();
            return Ok(data);
        }

        [HttpGet("latest")]
        public IActionResult GetLatestData()
        {
            var data = _dataService.GetLatestData();
            if (data == null)
            {
                return NotFound("No data available");
            }
            return Ok(data);
        }
    }
}