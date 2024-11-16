using System.ComponentModel.DataAnnotations;

namespace HomeEstate.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
