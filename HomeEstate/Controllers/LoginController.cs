using Microsoft.AspNetCore.Mvc;
using HomeEstate.Models;

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
            bool accountExists = LoginModel.CheckLogin(model.Username, model.Password);    //sending values entered for Username and Password into CheckLogin

            if (ModelState.IsValid)
            {
                

                if (model.Username == "admin" && model.Password == "password")  //change to where admin and password are in database table, and instead of model.Username, model.Password u are taking from the broker sign in object
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }



        // Register GET action
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
       
        public IActionResult RestartPassword()
        {
            return View();
        }



        // Register POST action
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Registration successful!";
                return RedirectToAction("Login");
            }

            return View(model);
        }

            
        [HttpPost]
        public IActionResult ForgetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // Placeholder logic for processing the username
                // You would normally verify if the username exists in the database
                bool userExists = model.UserName == "testuser"; // Example: replace with actual database check

                if (userExists)
                {
                    // Logic to send a password reset link or instructions
                    TempData["Message"] = "Password reset instructions sent to the email associated with this username.";
                }
                else
                {
                    ModelState.AddModelError("", "Username not found.");
                }
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult RestartPassword(string username)
        {
            // Pass the username to the RestartPassword view (if needed)
            ViewBag.Username = username;
            return View();
        }

    }
}
