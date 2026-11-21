
using Microsoft.AspNetCore.Mvc;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineesController : ControllerBase
    {
        private readonly TraineeService _service;

        public TraineesController(TraineeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search)
        {
            var trainees = await _service.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                trainees = trainees
                    .Where(t => t.FullName.Contains(
                        search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Ok(trainees.Select(t => new
            {
                t.TraineeId,
                t.FullName,
                t.PhoneNumber,
                t.Email
            }));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int id)
        {
            var t = await _service.GetByIdAsync(id);

            if (t == null)
                return NotFound();

            return Ok(new
            {
                t.TraineeId,
                t.FullName,
                t.PhoneNumber,
                t.Email
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] TraineeRequest request)
        {
            var t = await _service.AddAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = t.TraineeId },
                new
                {
                    t.TraineeId,
                    t.FullName,
                    t.PhoneNumber,
                    t.Email
                });
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] TraineeRequest request)
        {
            var success = await _service.UpdateAsync(id, request);

            return success ? NoContent() : NotFound();
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
