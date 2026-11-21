
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        public int TraineeId { get; set; }

        public int WorkshopId { get; set; }

        public DateTime RegistrationDate { get; set; }
            = DateTime.UtcNow;

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Confirmed";

        public Trainee Trainee { get; set; } = null!;

        public Workshop Workshop { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; }
            = new List<Attendance>();
    }
}
