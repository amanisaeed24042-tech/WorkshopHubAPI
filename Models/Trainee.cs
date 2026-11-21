
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.Models
{
    public class Trainee
    {
        [Key]
        public int TraineeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // One-to-One
        public TraineeProfile? TraineeProfile { get; set; }

        // One-to-Many
        public ICollection<Registration> Registrations { get; set; }
            = new List<Registration>();
    }
}
