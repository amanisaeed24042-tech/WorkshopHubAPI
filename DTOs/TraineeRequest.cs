
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.DTOs
{
    public class TraineeRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
