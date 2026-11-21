
using Microsoft.AspNetCore.Mvc;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopSessionsController : ControllerBase
    {
        private readonly WorkshopSessionService _service;

        public WorkshopSessionsController(
            WorkshopSessionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? workshopId)
        {
            var sessions = await _service.GetAllAsync(workshopId);

            return Ok(sessions.Select(s => new
            {
                s.WorkshopSessionId,
                s.WorkshopId,
                s.SessionDate,
                s.StartTime,
                s.EndTime,
                s.Topic
            }));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int id)
        {
            var s = await _service.GetByIdAsync(id);
            if (s == null) return NotFound();

            return Ok(new
            {
                s.WorkshopSessionId,
                s.WorkshopId,
                s.SessionDate,
                s.StartTime,
                s.EndTime,
                s.Topic
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] WorkshopSessionRequest request)
        {
            try
            {
                var s = await _service.AddAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id = s.WorkshopSessionId },
                    new
                    {
                        s.WorkshopSessionId,
                        s.WorkshopId,
                        s.Topic
                    });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] WorkshopSessionRequest request)
        {
            try
            {
                bool success = await _service.UpdateAsync(id, request);
                return success ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int id)
        {
            try
            {
                bool success = await _service.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
