
using Microsoft.AspNetCore.Mvc;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopsController : ControllerBase
    {
        private readonly WorkshopService _service;

        public WorkshopsController(WorkshopService service)
        {
            _service = service;
        }

        private static object ToResponse(Workshop w) => new
        {
            w.WorkshopId,
            w.Title,
            w.Description,
            w.StartDate,
            w.EndDate,
            w.Capacity,
            w.Price,
            w.TrainerId,
            TrainerName = w.Trainer?.FullName
        };

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search)
        {
            var workshops = await _service.GetAllAsync(search);

            return Ok(workshops.Select(w => ToResponse(w)));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int id)
        {
            var workshop = await _service.GetByIdAsync(id);

            if (workshop == null)
                return NotFound();

            return Ok(ToResponse(workshop));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] WorkshopRequest request)
        {
            try
            {
                var workshop = await _service.AddAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = workshop.WorkshopId },
                    new
                    {
                        workshop.WorkshopId,
                        workshop.Title,
                        workshop.TrainerId
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
            [FromBody] WorkshopRequest request)
        {
            try
            {
                var success = await _service.UpdateAsync(id, request);
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
                var success = await _service.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
