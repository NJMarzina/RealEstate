using System.Collections.Generic;

namespace HomeLibrary
{
    public class HomeDetails
    {
        public int HomeId { get; set; }
        public string AddressNumber { get; set; }
        public string AddressName { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressZip { get; set; }
        public string PropertyType { get; set; }
        public string Heating { get; set; }
        public string Cooling { get; set; }
        public int YearBuild { get; set; }
        public string Garage { get; set; }
        public string Utilities { get; set; }
        public string? Description { get; set; }
        public decimal AskingPrice { get; set; }
        public string Status { get; set; }
        public string WorkAddressName { get; set; }
        public string WorkAddressNumber { get; set; }
        public string WorkEmail { get; set; }
        public string RealEstateCompany { get; set; }
        public string CompanyPhone { get; set; }
        public int BedroomCount { get; set; }
        public int BathroomCount { get; set; }

        // List of rooms
        public List<RoomDetails> Rooms { get; set; } = new List<RoomDetails>();

        // List of amenities
        public List<string> Amenities { get; set; } = new List<string>();

        public List<HomeImageDetails> Images { get; set; } = new List<HomeImageDetails>();
    }

    public class RoomDetails
    {
        public string RoomType { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
    }



    public class HomeImageDetails
    {
        public string ImageUrl { get; set; } // URL or path to the image
        public string Description { get; set; } // Optional description of the image
    }

}
