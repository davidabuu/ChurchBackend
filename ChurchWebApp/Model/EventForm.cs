﻿using System.ComponentModel.DataAnnotations;

namespace ChurchWebApp.Model
{
    public class EventForm
    {
        public int Id { get; set; }
        [Required]
        public string? Day { get; set; }
        [Required]
        public string? Month { get; set; }
        [Required]
        public string? EventName { get; set; }
        [Required]
        public string? EventTime { get; set; }
        [Required]
        public string? EventLocation { get; set; }
    }
}