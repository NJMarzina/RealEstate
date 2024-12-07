namespace HomeEstate.Models
{
    public class HomeComparisonModel
    {
        public int homeId { get; set; }
        public int myBed { get; set; }
        public int myBath { get; set; }
        public int myPrice { get; set; }
        public int avgBed { get; set; }
        public int avgBath { get; set; }
        public double avgPrice { get; set; }
        public double expRent { get; set; }
        public String myCity { get; set; }
        public String myAddress { get; set; }
    }
}
