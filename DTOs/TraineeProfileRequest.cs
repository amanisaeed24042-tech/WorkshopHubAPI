
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.DTOs
{
    public class TraineeProfileRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TraineeId { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? EducationLevel { get; set; }

        public string? Notes { get; set; }
    }
}
