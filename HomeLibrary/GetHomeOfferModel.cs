using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLibrary
{
    public class GetHomeOfferModel
    {
        public int HomeId { get; set; }
        public string Address { get; set; }
        public int AskingPrice { get; set; }
        public string OfferName { get; set; }
        public string OfferEmail { get; set; }
        public string OfferPhone { get; set; }
        public int OfferAmount { get; set; }
        public string SaleType { get; set; }
        public string Contingencies { get; set; }
        public string NeedsToSellHome { get; set; }
        public string PreferredMoveInDate { get; set; }
    }
}
