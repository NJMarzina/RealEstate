using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLibrary
{
    public class EditHomeModel
    {
        public int HomeId { get; set; }
        public int AddressNumber { get; set; }
        public string AddressName { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public int AddressZip { get; set; }
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
