using HomeEstate.Models;
using HomeEstate.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
//using RealEstate.Models;
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
using static System.Net.WebRequestMethods;
using System.Reflection;
using System;
//using WebApi.Models;

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
        public IActionResult Requests(int HomeId)
        {
            HomeShowingModel showing = new HomeShowingModel
            {
                homeid = HomeId,
                ShowingDate = DateTime.Today.ToString(),
                BuyerName = string.Empty,
                BuyerPhone = string.Empty,
                BuyerEmail = string.Empty,
            };
            ViewBag.HomeId2 = HomeId;
            Debug.WriteLine(HomeId);
            return View("~/Views/Home/HomeShowing.cshtml", showing);
        }

        [HttpGet]
        public IActionResult Offer(int HomeId)
        {
            HomeOfferModel Offer = new HomeOfferModel
            {
                HomeId = HomeId,
                OfferName = string.Empty,
                OfferAmount = 0,
                OfferEmail = string.Empty,
                OfferPhone = string.Empty,
                Contingencies=string.Empty,
                NeedsToSellHome=string.Empty,
                PreferredMoveInDate=string.Empty,
                SaleType=string.Empty,
                
            };
            ViewBag.HomeId2 = HomeId;
            Debug.WriteLine(HomeId);
            return View("~/Views/Home/HomeOffer.cshtml",Offer);
        }

        [HttpPost]
        public IActionResult HomeOffer(HomeOfferModel homeOffer)
        {
            HomeOfferModel offer = new HomeOfferModel
            {
                
                OfferName = homeOffer.OfferName,
                OfferEmail = homeOffer.OfferEmail,
                OfferPhone = homeOffer.OfferPhone,
                OfferAmount = homeOffer.OfferAmount,
                HomeId = homeOffer.HomeId,
                SaleType = homeOffer.SaleType,
                Contingencies = homeOffer.Contingencies,
                NeedsToSellHome = homeOffer.NeedsToSellHome,
                PreferredMoveInDate = homeOffer.PreferredMoveInDate
            };
            string url = "https://localhost:7285/api/Home/AddHomeOffer/";
            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(offer);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonCustomer);
            writer.Flush();
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream theDataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(theDataStream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpPost]
        public IActionResult HomeShowing(HomeShowingModel Homeshowing)
        {
            HomeShowingModel hs = new HomeShowingModel();
            hs.BuyerName = Homeshowing.BuyerName;
            hs.BuyerPhone= Homeshowing.BuyerPhone;
            hs.ShowingDate = Homeshowing.ShowingDate;
            hs.BuyerEmail= Homeshowing.BuyerEmail;
            hs.homeid = Homeshowing.homeid;
            string url = "https://localhost:7285/api/Home/AddHomeShowing/";
            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(hs);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonCustomer);
            writer.Flush();
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream theDataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(theDataStream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return View(Homeshowing);
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
        
        [HttpPost]
        public IActionResult CreateNewHome(Home home)
        {
            var brokerID = Request.Cookies["BrokerID"];
            var profileID = Request.Cookies["ProfileID"];

            /*
            WebApi.Models.HomeModel newHome = new WebApi.Models.HomeModel();
            newHome.AddressNumber = home.AddressNumber;
            newHome.AddressName = home.AddressName;
            newHome.AddressCity = home.AddressCity;
            newHome.AddressState = home.AddressState;
            newHome.AddressZip = home.AddressZip;
            newHome.PropertyType = home.PropertyType;
            newHome.Heating = home.Heating;
            newHome.Cooling = home.Cooling;
            newHome.YearBuild = home.YearBuild;
            newHome.Garage = home.Garage;
            newHome.Utilities = home.Utilities;
            newHome.Description = home.Description;
            newHome.AskingPrice = home.AskingPrice;
            newHome.Status = home.Status;
            */

            string webApiUrl = "https://localhost:7285/api/Home/CreateNewHome/";

            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonHome = js.Serialize(home);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApiUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonHome);
            writer.Flush();
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();  //400 error here
            //i think the error is due to a "home" mismatch of sending different versions of home and homemodel somewhere
            Stream theDataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(theDataStream);
            String data = reader.ReadToEnd();
            reader.Close();
            response.Close();


            return View();
        }

    }
}
