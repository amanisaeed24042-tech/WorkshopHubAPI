
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkshopHubAPI.DTOs
{
    public class WorkshopRequest
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(1, 500)]
        public int Capacity { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int TrainerId { get; set; }
    }
}
