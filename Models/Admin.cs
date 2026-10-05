using System.ComponentModel.DataAnnotations;

namespace websiteCofee.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
