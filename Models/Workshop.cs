
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopHubAPI.Models
{
    public class Workshop
    {
        [Key]
        public int WorkshopId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, 500)]
        public int Capacity { get; set; }

        [Range(0, 100000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Foreign Key
        public int TrainerId { get; set; }

        // Navigation Property
        [ForeignKey(nameof(TrainerId))]
        public Trainer Trainer { get; set; } = null!;

        // One-to-Many relationships
        public ICollection<WorkshopSession> Sessions { get; set; }
            = new List<WorkshopSession>();

        public ICollection<Registration> Registrations { get; set; }
            = new List<Registration>();
    }
}
