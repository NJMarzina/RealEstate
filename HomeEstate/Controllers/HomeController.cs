using HomeEstate.Models;
using HomeEstate.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RealEstate.Models;
using System.Data;
using System.Diagnostics;


namespace HomeEstate.Controllers
{
    public class HomeController : Controller
    {
        /* private readonly ILogger<HomeController> _logger;

         public HomeController(ILogger<HomeController> logger)
         {
             _logger = logger;
         }*/
        public IActionResult Dashboard()
        {
            DBConnect objDB = new DBConnect();
            DataSet ds = objDB.GetDataSet(@"SELECT Address_Number, Address_Name, AddressCity, AddressState, AddressZip, 
                                           Property_Type, Heating, Cooling, Year_Build, Garage, 
                                           Utilities, Description, AskingPrice, Status 
                                    FROM Home");

            List<Home> homes = new List<Home>();

            foreach (DataRow record in ds.Tables[0].Rows)
            {
                var home = new Home
                {
                    Address_Number = record["Address_Number"].ToString(),
                    Address_Name = record["Address_Name"].ToString(),
                    AddressCity = record["AddressCity"].ToString(),
                    AddressState = record["AddressState"].ToString(),
                    AddressZip = record["AddressZip"].ToString(),
                    Property_Type = record["Property_Type"].ToString(),
                     Heating = record["Heating"].ToString(),
                    Cooling = record["Cooling"].ToString(),
                    Year_Build = Convert.ToInt32(record["Year_Build"].ToString()),
                    Garage = record["Garage"].ToString(),
                    Utilities = record["Utilities"].ToString(),
                    Description = record["Description"].ToString(),
                    AskingPrice = Convert.ToInt32(record["AskingPrice"].ToString()),
                    Status = record["Status"].ToString()
                };

                homes.Add(home);
            }

            ViewBag.HomeList = homes;  // Using ViewBag to pass data to the view

            return View("Dashboard");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
