namespace RealEstate.Models
{
    public class Home
    {
        public int HomeID { get; set; }
        public int ProfileID { get; set; }
        public string? AddressNumber { get; set; }
        public string? AddressName { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressZip { get; set; }
        public string? PropertyType { get; set; }
        public double? Size { get; set; }
        public string? Heating { get; set; }
        public string? Cooling { get; set; }
        public int? YearBuild { get; set; }
        public string? Garage { get; set; }
        public string? Utilities { get; set; }
        public string? Description { get; set; }
        public int? AskingPrice { get; set; }
        public string? Status { get; set; }
    }
}
