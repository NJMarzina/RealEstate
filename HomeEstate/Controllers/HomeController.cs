using HomeEstate.Models;
using HomeEstate.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RealEstate.Models;
using System.Data;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Net;
using Newtonsoft.Json.Converters;



namespace HomeEstate.Controllers
{
    public class HomeController : Controller
    {

        Uri address = new Uri("https://cis-iis2.temple.edu/Fall2024/cis3342_tun52511/TermProject/api");
        Uri webApiUrl = new Uri("https://localhost:7229/api");
        /* private readonly ILogger<HomeController> _logger;

         public HomeController(ILogger<HomeController> logger)
         {
             _logger = logger;
         }*/
        public IActionResult Dashboard()
        {
            List<Home> homes = new List<Home>();
          
            
            Uri webApiUrl = new Uri("https://localhost:7229/api/Home/GetHomeData"); 
            WebRequest request = WebRequest.Create(webApiUrl);
            WebResponse response = request.GetResponse();

            Stream theDataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(theDataStream);

            String data = reader.ReadToEnd();

            reader.Close();

            response.Close();









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
