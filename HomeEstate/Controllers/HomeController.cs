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
using System.Runtime.InteropServices;
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

        [HttpGet]
        public IActionResult Comparisons(int HomeId)
        {
            HomeComparisonModel details = new HomeComparisonModel
            {
                homeId = HomeId,
                myBed = 0,
                myBath = 0,
                myPrice = 0,
                avgBed = 0,
                avgBath = 0,
                avgPrice = 0.0,
                expRent = 0,
                myCity = string.Empty,
                myAddress = string.Empty,
            };

            //* from Home where City = City

            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetHomeInfo";

            objCommand.Parameters.AddWithValue("@Home_ID", HomeId);

            DataSet homeDS = objDB.GetDataSet(objCommand);

            details.myPrice = int.Parse(homeDS.Tables[0].Rows[0]["AskingPrice"].ToString());
            details.myCity = homeDS.Tables[0].Rows[0]["AddressCity"].ToString();
            details.myAddress = homeDS.Tables[0].Rows[0]["Address_Number"].ToString() + " " + homeDS.Tables[0].Rows[0]["Address_Name"].ToString();

            DBConnect objDB2 = new DBConnect();
            SqlCommand objCommand2 = new SqlCommand();
            objCommand2.CommandType = CommandType.StoredProcedure;
            objCommand2.CommandText = "GetHomesByCityUpdated";

            objCommand2.Parameters.AddWithValue("@City", details.myCity);

            DataSet homesInCityDS = objDB.GetDataSet(objCommand2);

            double totalPrice = 0;
            int homeCount = 0;

            foreach (DataRow row in homesInCityDS.Tables[0].Rows)
            {
                double currentPrice = Convert.ToDouble(row["AskingPrice"]);
                totalPrice += currentPrice;
                homeCount++;
            }

            details.avgPrice = totalPrice / homeCount;

            details.expRent = (details.myPrice / 360) * 1.04;

            return View("~/Views/Home/Comparisons.cshtml", details);
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
        

        public IActionResult CreateNewHome()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateNewHome(AddHomeModel home)
        {
            var brokerID = Request.Cookies["BrokerID"];
            var profileID = Request.Cookies["ProfileID"];
            AddHomeModel homeModel = new AddHomeModel();

            homeModel.ProfileID =Convert.ToInt32( Request.Cookies["ProfileID"]);
            homeModel.AddressNumber = home.AddressNumber;
            homeModel.AddressName = home.AddressName;
            homeModel.AddressCity = home.AddressCity;
            homeModel.AddressState = home.AddressState;
            homeModel.AddressZip = home.AddressZip;
            homeModel.PropertyType = home.PropertyType;
            homeModel.Size = home.Size;
            homeModel.Heating = home.Heating;
            homeModel.Cooling = home.Cooling;
            homeModel.YearBuild = home.YearBuild;
            homeModel.Garage = home.Garage;
            homeModel.Utilities = home.Utilities;
            homeModel.Description = home.Description;
            homeModel.AskingPrice = home.AskingPrice;
            homeModel.Status = home.Status;

            homeModel.AmenitiesName = home.AmenitiesName;
           
            string url = "https://localhost:7285/api/Home/CreateNewHome/";

            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(homeModel);

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

            //send to api
            //add to ammentities table

            return RedirectToAction("BrokerDashboard", "Broker");
        }

    }
}
