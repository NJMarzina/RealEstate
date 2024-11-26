using Microsoft.AspNetCore.Mvc;

namespace HomeEstate.Controllers
{
    public class BrokerController : Controller
    {
        public IActionResult BrokerDashboard()
        {
            // Retrieve the 'Username' cookie safely
            var username = Request.Cookies["Username"];

            // If the cookie doesn't exist, set a default value (e.g., "Guest")
            if (username == null)
            {
                username = "Guest";
            }

            // Pass the username to the view using ViewData
            ViewData["Username"] = username;

            return View("BrokerDashboard");
        }
    }
}
