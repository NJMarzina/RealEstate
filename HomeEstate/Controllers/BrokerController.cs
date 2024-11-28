using HomeEstate.Models;
using HomeLibrary;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Net;
using static Azure.Core.HttpHeader;
using HomeLibrary;
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
    
    }

    }

