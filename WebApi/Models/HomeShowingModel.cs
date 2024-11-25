namespace WebApi.Models
{
    public class HomeShowingModel
    {
        public string BuyerName { get; set; } // Name of the buyer
        public string BuyerEmail { get; set; } // Email address of the buyer
        public string BuyerPhone { get; set; } // Phone number of the buyer
        public String ShowingDate { get; set; } // Date of the showing
        public int HomeId { get; set; } // ID of the home being shown

    }
}
