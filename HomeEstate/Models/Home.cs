using System.ComponentModel.DataAnnotations;

namespace HomeEstate.Models
{
    public class Home
    {
        public int HomeID { get; set; }
        public int ProfileID { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string AddressNumber { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string AddressName { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string AddressCity { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string AddressState { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string AddressZip { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string PropertyType { get; set; }

        [Required(ErrorMessage = "Required.")]
        public float Size { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Heating { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Cooling { get; set; }

        [Required(ErrorMessage = "Required.")]
        public int YearBuild { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Garage { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Utilities { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required.")]
        public int AskingPrice { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Status { get; set; }
        public string ImageUrl { get; set; }

    }
}
