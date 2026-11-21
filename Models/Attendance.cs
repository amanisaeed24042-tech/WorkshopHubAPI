
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int RegistrationId { get; set; }

        public int WorkshopSessionId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Absent";

        public string? Notes { get; set; }

        public Registration Registration { get; set; } = null!;

        public WorkshopSession WorkshopSession { get; set; } = null!;
    }
}
