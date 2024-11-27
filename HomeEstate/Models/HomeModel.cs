namespace WebApi.Models
{
    public class HomeModel
    {
        public int homeId { get; set; }
        public string addressNumber { get; set; }
        public string addressName { get; set; }
        public string addressCity { get; set; }
        public string addressState { get; set; }
        public string addressZip { get; set; }
        public string propertyType { get; set; }
        public float size { get; set; }
        public string heating { get; set; }
        public string cooling { get; set; }
        public int yearBuild { get; set; }
        public string garage { get; set; }
        public string utilities { get; set; }
        public string description { get; set; }
        public int askingPrice { get; set; }
        public string status { get; set; }
    }
}
