namespace HomeEstate.Models
{
    public class HomeOfferModel
    {
        public string OfferName { get; set; } // Name of the person making the offer
        public string OfferEmail { get; set; } // Email of the person making the offer
        public string OfferPhone { get; set; } // Phone number of the person making the offer
        public int OfferAmount { get; set; } // Amount of the offer
        public int HomeId { get; set; } // Identifier for the home being offered on
        public string SaleType { get; set; } // Type of sale (e.g., cash, financing)
        public string Contingencies { get; set; } // Contingencies for the offer
        public string NeedsToSellHome { get; set; } // Whether the buyer needs to sell a home (e.g., Yes/No)
        public string PreferredMoveInDate { get; set; } // Preferred move-in date
    }
}
