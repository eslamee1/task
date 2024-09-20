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

        public DateTime DateAndTime { get; set; }

        [NotMapped]
        public string[] SelectedMonths { get; set; } = new string[0];
        [NotMapped]
        public int[] SelectedDays { get; set; } = new int[0];
        [NotMapped]
        public string[] SelectedWeekDays { get; set; } = new string[0];

        // Updated to use a List of Rating objects
        public List<Rating> RatingList { get; set; } = new List<Rating>();

        public string Comments { get; set; }
    }
}
