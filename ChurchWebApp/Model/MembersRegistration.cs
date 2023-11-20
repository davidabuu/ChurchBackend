using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchWebApp.Model
{
    public class MembersRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set;}

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Organization { get; set; }
        [Required]

        public string? Sex { get; set; }

        [Required]
        public int Age { get; set; }
        public bool Paid { get; set; }

        public string? TnxRef { get; set; }
    }
}
