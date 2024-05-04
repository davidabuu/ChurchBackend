using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchWebApp.Model
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id { get; set; }
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

        public string? EventDescription { get; set; }   
    }
}
