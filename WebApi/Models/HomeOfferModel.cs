namespace WebApi.Models
{
    public class HomeOfferModel
    {
        public string OfferName { get; set; } 
        public string OfferEmail { get; set; } 
        public string OfferPhone { get; set; } 
        public int OfferAmount { get; set; } 
        public int HomeId { get; set; } 
        public string SaleType { get; set; } 
        public string Contingencies { get; set; } 
        public string NeedsToSellHome { get; set; } 
        public string PreferredMoveInDate { get; set; } 
    }
}
