using HomeEstate.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeEstate.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (model.Username == "admin" && model.Password == "password")
                {
                    return RedirectToAction("Index", "Home"); 
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }




    }
}
