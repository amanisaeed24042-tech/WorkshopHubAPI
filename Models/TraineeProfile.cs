
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.Models
{
    public class TraineeProfile
    {
        [Key]
        public int TraineeProfileId { get; set; }

        public int TraineeId { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? EducationLevel { get; set; }

        public string? Notes { get; set; }

        // Navigation Property
        public Trainee Trainee { get; set; } = null!;
    }
}
