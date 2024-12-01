using HomeEstate.Models;
using HomeLibrary;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Net;
using static Azure.Core.HttpHeader;
using HomeLibrary;
using System;
using System.Diagnostics;
//using WebApi.Models;

namespace HomeEstate.Controllers
{
    public class BrokerController : Controller
    {
        public IActionResult BrokerDashboard()
        {
            var username = Request.Cookies["Username"];
            var brokerid = Request.Cookies["BrokerID"];
            var profileid = Request.Cookies["ProfileID"];

            ViewData["Username"] = username;
            ViewData["BrokerID"] = brokerid;
            ViewData["ProfileID"] = profileid;

            return View("BrokerDashboard");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Password");
            Response.Cookies.Delete("BrokerID");
            Response.Cookies.Delete("ProfileID");

            return RedirectToAction("Dashboard", "Home");
        }

        public IActionResult BrokerListing(int id)
        {
            String webApiUrl = "https://localhost:7285/api/BrokerUser/GetHomeByBroker/" + id;


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
            List<HomeModel> home = js.Deserialize<List<HomeModel>>(data);
            ViewBag.HomeList = home;

            if (home == null)
            {
                return NotFound("Home details not found.");
            }

            return View("BrokerListing");
        }
  
    public IActionResult BrokerOffer(int id)
        {
            String webApiUrl = "https://localhost:7285/api/BrokerUser/GetOfferByBroker/" + id;
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
            List<GetHomeOfferModel> home = js.Deserialize<List<GetHomeOfferModel>>(data);
            ViewBag.HomeList = home;
            if (home == null)
            {
                return NotFound("No Offer found.");
            }
            return View("BrokerOffer");

        }
        public IActionResult BrokerShowing(int id)
        {

            String webApiUrl = "https://localhost:7285/api/BrokerUser/GetShowingByBroker/" + id;

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
            List<GetHomeShowingModel> home = js.Deserialize<List<GetHomeShowingModel>>(data);
            ViewBag.HomeList = home;
            if (home == null)
            {
                return NotFound("No Showing found.");
            }
            return View("BrokerShowing");
        }



        public IActionResult EditHomes(int id)
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
            EditHomeModel home = js.Deserialize<EditHomeModel>(data);

            if (home == null)
            {
                return NotFound("Home details not found.");
            }

            return View("EditHomes", home);
        }




        [HttpPost]
        public IActionResult UpdateHome(EditHomeModel home)
        {
            EditHomeModel updatedHome = new EditHomeModel();

            updatedHome.HomeId = home.HomeId;
            updatedHome.AddressNumber = home.AddressNumber;
            updatedHome.AddressName = home.AddressName;
            updatedHome.AddressCity = home.AddressCity;
            updatedHome.AddressState = home.AddressState;
            updatedHome.AddressZip = home.AddressZip;
            updatedHome.PropertyType = home.PropertyType;
            updatedHome.Heating = home.Heating;
            updatedHome.Cooling = home.Cooling;
            updatedHome.YearBuild = home.YearBuild;
            updatedHome.Garage = home.Garage;
            updatedHome.Utilities = home.Utilities;
            updatedHome.Description = home.Description;
            updatedHome.AskingPrice = home.AskingPrice;
            updatedHome.Status = home.Status;
            
          
            string url = "https://localhost:7285/api/BrokerUser/EditHome/";
            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(updatedHome);
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
            return RedirectToAction("BrokerDashboard", "Broker");
        }



        [HttpPost]
        public IActionResult DeleteHome(int id)
        {
            String webApiUrl = "https://localhost:7285/api/Broker/DeleteHome/"+id;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApiUrl);
            request.Method = "DELETE";
            request.ContentType = "application/json";

            
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
             
                response.Close();
            
            

            // Redirect back to the Broker Listing view
            var brokerId = Request.Cookies["BrokerID"];
            return RedirectToAction("BrokerListing", new { id = brokerId });
        }

        [HttpGet]
        public IActionResult RequestID(int HomeId)
        {
            RoomModel room = new RoomModel
            {
                RoomId = HomeId,
                Width=null,
                Length=null,
                RoomType=string.Empty

            };          
            ViewBag.HomeId2 = HomeId;
            Debug.WriteLine(HomeId);
            return View("~/Views/Broker/AddRoom.cshtml", room);
        }


        [HttpPost]
        public IActionResult AddRoom(RoomModel Room) {
            RoomModel UpdateRoom = new RoomModel();
            UpdateRoom.RoomId = Room.RoomId;
            UpdateRoom.RoomType=Room.RoomType;
            UpdateRoom.Width = Room.Width;
            UpdateRoom.Length = Room.Length;
            string url = "https://localhost:7285/api/BrokerUser/AddRoom/";
            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(UpdateRoom);
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
            return RedirectToAction("BrokerDashboard", "Broker");
        }




    }

}

