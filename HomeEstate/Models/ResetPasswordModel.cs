using System.ComponentModel.DataAnnotations;

namespace HomeEstate.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public  String UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
