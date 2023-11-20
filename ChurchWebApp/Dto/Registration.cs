using System.ComponentModel.DataAnnotations;

namespace ChurchWebApp.Dto
{
    public class Registration
    {

        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
       
    }
}
