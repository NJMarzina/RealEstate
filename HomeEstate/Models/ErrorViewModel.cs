using Microsoft.Data.SqlClient;

namespace HomeEstate.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        


        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
