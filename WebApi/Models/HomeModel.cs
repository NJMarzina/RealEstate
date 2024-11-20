namespace WebApi.Models
{
    public class HomeModel
    {
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
        public string Description { get; set; }
        public decimal AskingPrice { get; set; }
        public string Status { get; set; }
    }
}
