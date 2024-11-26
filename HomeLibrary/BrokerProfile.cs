using System.ComponentModel.DataAnnotations;

namespace HomeLibrary
{
    public class BrokerProfile
    {

        public int BrokerId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string HomeEmail { get; set; }

        [StringLength(50)]
        public string AddressName { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        // BrokerProfile fields
        public string WorkAddressName { get; set; }
        public string WorkAddressNumber { get; set; }
        [EmailAddress]
        public string WorkEmail { get; set; }
        public string RealEstateCompany { get; set; }
        public string CompanyPhone { get; set; }

        [Required]
        [Display(Name = "Security Question 1")]
        public required string SecurityQuestion1 { get; set; }

        [Required]
        [Display(Name = "Security Answer 1")]
        public required string SecurityAnswer1 { get; set; }

        [Required]
        [Display(Name = "Security Question 2")]
        public required string SecurityQuestion2 { get; set; }

        [Required]
        [Display(Name = "Security Answer 2")]
        public required string SecurityAnswer2 { get; set; }

        [Required]
        [Display(Name = "Security Question 3")]
        public required string SecurityQuestion3 { get; set; }

        [Required]
        [Display(Name = "Security Answer 3")]
        public required string SecurityAnswer3 { get; set; }
    }

}

