
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.DTOs
{
    public class AttendanceRequest
    {
        [Range(1, int.MaxValue)]
        public int RegistrationId { get; set; }

        [Range(1, int.MaxValue)]
        public int WorkshopSessionId { get; set; }

        [Required]
        public string Status { get; set; } = "Absent";

        public string? Notes { get; set; }
    }
}
