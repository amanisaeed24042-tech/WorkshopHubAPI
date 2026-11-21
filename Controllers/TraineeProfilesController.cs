
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Services;

namespace WorkshopHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineeProfilesController : ControllerBase
    {
        private readonly TraineeProfileService _service;

        public TraineeProfilesController(TraineeProfileService service)
        {
            _service = service;
        }

        [HttpGet("trainee/{traineeId:int:min(1)}")]
        public async Task<IActionResult> GetByTraineeId(
            [FromRoute] int traineeId)
        {
            var p = await _service.GetByTraineeIdAsync(traineeId);

            if (p == null)
                return NotFound();

            return Ok(new
            {
                p.TraineeProfileId,
                p.TraineeId,
                p.Address,
                p.EducationLevel,
                p.Notes
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] TraineeProfileRequest request)
        {
            try
            {
                var p = await _service.AddAsync(request);

                return CreatedAtAction(
                    nameof(GetByTraineeId),
                    new { traineeId = p.TraineeId },
                    new
                    {
                        p.TraineeProfileId,
                        p.TraineeId,
                        p.Address,
                        p.EducationLevel,
                        p.Notes
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
                    message = "Profile already exists or conflicts with database data."
                });
            }
        }

        [HttpPut("trainee/{traineeId:int:min(1)}")]
        public async Task<IActionResult> Update(
            [FromRoute] int traineeId,
            [FromBody] TraineeProfileRequest request)
        {
            try
            {
                bool updated = await _service.UpdateAsync(
                    traineeId, request);

                return updated ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("trainee/{traineeId:int:min(1)}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int traineeId)
        {
            bool deleted = await _service.DeleteAsync(traineeId);

            return deleted ? NoContent() : NotFound();
        }
    }
}
