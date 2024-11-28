using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLibrary
{
    public class GetHomeShowingModel
    {
        public int ShowingId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerPhone { get; set; }
        public string ShowingDate { get; set; }
        public int HomeId { get; set; }
        public string Address { get; set; } // Combination of Address_Number and Address_Name
        public string PropertyType { get; set; }
        public int AskingPrice { get; set; }
    }
}
