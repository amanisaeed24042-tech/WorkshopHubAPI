
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.DTOs
{
    public class WorkshopSessionRequest
    {
        [Range(1, int.MaxValue)]
        public int WorkshopId { get; set; }

        public DateTime SessionDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [Required]
        [MaxLength(150)]
        public string Topic { get; set; } = string.Empty;
    }
}
