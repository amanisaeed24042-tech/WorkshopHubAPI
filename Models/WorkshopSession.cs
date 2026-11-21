
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.Models
{
    public class WorkshopSession
    {
        [Key]
        public int WorkshopSessionId { get; set; }

        public int WorkshopId { get; set; }

        public DateTime SessionDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [Required]
        [MaxLength(150)]
        public string Topic { get; set; } = string.Empty;

        public Workshop Workshop { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; }
            = new List<Attendance>();
    }
}

