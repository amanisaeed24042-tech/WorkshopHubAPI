
using Microsoft.AspNetCore.Mvc;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _service.GetStatsAsync();
            return Ok(result);
        }

        [HttpGet("upcoming-workshops")]
        public async Task<IActionResult> GetUpcomingWorkshops()
        {
            var result =
                await _service.GetUpcomingWorkshopsAsync();

            return Ok(result);
        }
    }
}
