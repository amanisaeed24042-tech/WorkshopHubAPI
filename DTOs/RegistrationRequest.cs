
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.DTOs
{
    public class RegistrationRequest
    {
        [Range(1, int.MaxValue)]
        public int TraineeId { get; set; }

        [Range(1, int.MaxValue)]
        public int WorkshopId { get; set; }
    }
}
