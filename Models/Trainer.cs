
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // One-to-Many Relationship
        public ICollection<Workshop> Workshops { get; set; }
            = new List<Workshop>();
    }
}
