namespace RealEstate.Models
{
    public class Home
    {
        public int Home_ID { get; set; }
        public int Profile_ID { get; set; }
        public string? Address_Number { get; set; }
        public string? Address_Name { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressZip { get; set; }
        public string? Property_Type { get; set; }
        public double? Size { get; set; }
        public string? Heating { get; set; }
        public string? Cooling { get; set; }
        public int? Year_Build { get; set; }
        public string? Garage { get; set; }
        public string? Utilities { get; set; }
        public string? Description { get; set; }
        public int? AskingPrice { get; set; }
        public string? Status { get; set; }
    }
}
