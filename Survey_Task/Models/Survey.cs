using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey_Task.Models
{
    public class Survey
    {
        public int Id { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string Park { get; set; }

        // Date and time based on the selected month and day
        public DateTime DateAndTime { get; set; }

        [NotMapped]
        public string SelectedMonth { get; set; }

        [NotMapped]
        public int SelectedDay { get; set; }


        public String SelectedWeekDay { get; set; }

        // Ratings list
        public List<Rating>? RatingList { get; set; } = new List<Rating>();

        public string? Comments { get; set; }
        public string? QRCodeUrl { get; set; }
    }
}
