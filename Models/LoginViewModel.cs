using System.ComponentModel.DataAnnotations;

namespace websiteCofee.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Fadlan geli Email-kaaga")]
        [EmailAddress(ErrorMessage = "Email-ku maahan mid sax ah")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fadlan geli Password-kaaga")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
