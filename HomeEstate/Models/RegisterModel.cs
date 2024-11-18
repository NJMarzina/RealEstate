using System.ComponentModel.DataAnnotations;

namespace HomeEstate.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string HomeEmail { get; set; }

        [Required(ErrorMessage = "Address name is required.")]
        public string AddressName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Address number must be greater than 0.")]
        public int AddressNumber { get; set; }

        [Required(ErrorMessage = "Answer to the first security question is required.")]
        public string SecurityAnswer1 { get; set; }

        [Required(ErrorMessage = "Answer to the second security question is required.")]
        public string SecurityAnswer2 { get; set; }

        [Required(ErrorMessage = "Answer to the third security question is required.")]
        public string SecurityAnswer3 { get; set; }
    }
}
