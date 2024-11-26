using Microsoft.AspNetCore.Mvc;

namespace HomeEstate.Controllers
{
    public class BrokerController : Controller
    {
        public IActionResult BrokerDashboard()
        {
            var username = Request.Cookies["Username"];
            var brokerid = Request.Cookies["BrokerID"];

            ViewData["Username"] = username;

            return View("BrokerDashboard");
        }
    }
}
