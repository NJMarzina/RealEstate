﻿using System.ComponentModel.DataAnnotations;

namespace HomeEstate.Models
{
    public class RegisterModel
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



        // Broker Profile Fields
        [StringLength(50)]
        public string WorkAddressName { get; set; }

        [StringLength(50)]
        public string WorkAddressNumber { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string WorkEmail { get; set; }

        [StringLength(50)]
        public string RealEstateCompany { get; set; }

        [Phone]
        [StringLength(50)]
        public string CompanyPhone { get; set; }


    }
}
