
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly RegistrationService _service;

        public RegistrationsController(
            RegistrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? workshopId)
        {
            var items = await _service.GetAllAsync(workshopId);

            return Ok(items.Select(r => new
            {
                r.RegistrationId,
                r.TraineeId,
                r.WorkshopId,
                r.RegistrationDate,
                r.Status
            }));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int id)
        {
            var r = await _service.GetByIdAsync(id);
            if (r == null) return NotFound();

            return Ok(new
            {
                r.RegistrationId,
                r.TraineeId,
                r.WorkshopId,
                r.RegistrationDate,
                r.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] RegistrationRequest request)
        {
            try
            {
                var r = await _service.AddAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id = r.RegistrationId },
                    new
                    {
                        r.RegistrationId,
                        r.TraineeId,
                        r.WorkshopId,
                        r.Status
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
                    message = "Registration conflicts with existing data."
                });
            }
        }

        [HttpPut("{id:int:min(1)}/cancel")]
        public async Task<IActionResult> Cancel(
            [FromRoute] int id)
        {
            try
            {
                bool success = await _service.CancelAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
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
