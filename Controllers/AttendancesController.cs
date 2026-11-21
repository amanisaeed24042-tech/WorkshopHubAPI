
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly AttendanceService _service;

        public AttendancesController(
            AttendanceService service)
        {
            _service = service;
        }

        [HttpGet("session/{sessionId:int:min(1)}")]
        public async Task<IActionResult> GetBySession(
            [FromRoute] int sessionId)
        {
            var items = await _service.GetBySessionAsync(sessionId);

            return Ok(items.Select(a => new
            {
                a.AttendanceId,
                a.RegistrationId,
                a.WorkshopSessionId,
                a.Status,
                a.Notes
            }));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int id)
        {
            var a = await _service.GetByIdAsync(id);
            if (a == null) return NotFound();

            return Ok(new
            {
                a.AttendanceId,
                a.RegistrationId,
                a.WorkshopSessionId,
                a.Status,
                a.Notes
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] AttendanceRequest request)
        {
            try
            {
                var a = await _service.AddAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id = a.AttendanceId },
                    new
                    {
                        a.AttendanceId,
                        a.RegistrationId,
                        a.WorkshopSessionId,
                        a.Status,
                        a.Notes
                    });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Attendance conflicts with existing data."
                });
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] AttendanceRequest request)
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
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Attendance conflicts with existing data."
                });
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int id)
        {
            bool success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
