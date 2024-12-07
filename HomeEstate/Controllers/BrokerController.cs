using HomeEstate.Models;
using HomeLibrary;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Net;
using System;
using System.Diagnostics;

namespace HomeEstate.Controllers
{
    public class BrokerController : Controller
    {
        public static string Publishapi = "https://cis-iis2.temple.edu/Fall2024/cis3342_tun52511/WebAPI";

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
            String webApiUrl = Publishapi + "/api/BrokerUser/GetHomeByBroker/" + id;

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
            String webApiUrl = Publishapi + "/api/BrokerUser/GetOfferByBroker/" + id;
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
            String webApiUrl = Publishapi + "/api/BrokerUser/GetShowingByBroker/" + id;

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
            String webApiUrl = Publishapi + "/api/Home/GetHomeDetails/" + id;
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
            if (Request.Form["_method"] == "PUT")
            {
                string url = Publishapi + "/api/BrokerUser/EditHome/";

                JavaScriptSerializer js = new JavaScriptSerializer();
                String jsonCustomer = js.Serialize(home);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonCustomer);
                writer.Flush();
                writer.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();

                return RedirectToAction("BrokerDashboard", "Broker");
            }
            return View();
        }

        [HttpPost]
        public IActionResult DeleteHome(int id)
        {
            String webApiUrl = Publishapi + "/api/BrokerUser/DeleteHome/" + id;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApiUrl);
            request.Method = "DELETE";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            var brokerId = Request.Cookies["BrokerID"];
            return RedirectToAction("BrokerListing", new { id = brokerId });
        }

        [HttpGet]
        public IActionResult RequestID(int HomeId)
        {
            RoomModel room = new RoomModel
            {
                RoomId = HomeId,
                Width = null,
                Length = null,
                RoomType = string.Empty
            };
            ViewBag.HomeId2 = HomeId;
            return View("~/Views/Broker/AddRoom.cshtml", room);
        }

        [HttpGet]
        public IActionResult RequestImageID(int HomeId)
        {
            AddImagiesModel Home = new AddImagiesModel
            {
                HomeId = HomeId,
                ImiageUrl = string.Empty
            };
            ViewBag.HomeId2 = HomeId;
            return View("~/Views/Broker/AddImage.cshtml", Home);
        }

        [HttpPost]
        public IActionResult AddImage(AddImagiesModel home)
        {
            string url = Publishapi + "/api/BrokerUser/AddImage/";

            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(home);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonCustomer);
            writer.Flush();
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            return RedirectToAction("BrokerDashboard", "Broker");
        }

        [HttpPost]
        public IActionResult AddRoom(RoomModel Room)
        {
            string url = Publishapi + "/api/BrokerUser/AddRoom/";

            JavaScriptSerializer js = new JavaScriptSerializer();
            String jsonCustomer = js.Serialize(Room);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonCustomer);
            writer.Flush();
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            return RedirectToAction("BrokerDashboard", "Broker");
        }
    }
}
