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
using Nancy;
using Nancy.Json;
using HomeLibrary;

namespace HomeEstate.Controllers
{
    public class HomeController : Controller
    {

        Uri address = new Uri("https://cis-iis2.temple.edu/Fall2024/cis3342_tun52511/TermProject/api");
        Uri webApiUrl = new Uri("https://localhost:7229/api");
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // HomeEstate.Controllers.HomeController.Dashboard() in HomeController.cs
        public IActionResult Dashboard()
        {

            String webApiUrl = "https://localhost:7285/api/Home/GetHomeData";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApiUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream theDataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(theDataStream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Home> homes = js.Deserialize<List<Home>>(data);
            ViewBag.HomeList = homes;
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


        [HttpGet]
        public IActionResult HomeDetails(int id)
        {
           
                String webApiUrl = "https://localhost:7285/api/Home/GetHomeDetails/" + id;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApiUrl);
                request.Method = "GET";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream theDataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(theDataStream);
                String data = reader.ReadToEnd();
                reader.Close();
                response.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                HomeDetails home = js.Deserialize<HomeDetails>(data);

                if (home == null)
                {
                    return NotFound("Home details not found.");
                }

                return View("HomeDetails", home);
            }
            
        }


    
}
