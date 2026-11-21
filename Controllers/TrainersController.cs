
using Microsoft.AspNetCore.Mvc;
using WorkshopHubAPI.Models;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly TrainerService _service;

        public TrainersController(TrainerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var trainer = await _service.GetByIdAsync(id);

            if (trainer == null)
                return NotFound();

            return Ok(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Trainer trainer)
        {
            var result = await _service.AddAsync(trainer);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.TrainerId },
                result
            );
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] Trainer trainer)
        {
            var updated = await _service.UpdateAsync(id, trainer);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
