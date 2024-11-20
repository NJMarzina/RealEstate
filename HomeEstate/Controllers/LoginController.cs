using Microsoft.AspNetCore.Mvc;
using HomeEstate.Models;
using System.Net;
using System.Text.Json;
using System.Text;


namespace HomeEstate.Controllers
{
    public class LoginController : Controller
    {
        Uri address = new Uri("https://cis-iis2.temple.edu/Fall2024/cis3342_tun52511/TermProject/api");
        Uri webApiUrl = new Uri("https://localhost:7229/api");


        [HttpPost]
        public async Task<IActionResult> CheckLogin(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var jsonLogin = JsonSerializer.Serialize(user);
                var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    try
                    {
                        // Use the correct API URL
                        var response = await client.PostAsync(webApiUrl + "/Login/Login", content);

                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the response as a boolean (true or false)
                            var data = await response.Content.ReadAsStringAsync();
                            bool isCorrect = bool.Parse(data);

                            if (isCorrect)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid credentials");
                                return View("Login", user);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error occurred while checking credentials.");
                            return View("Login", "Login");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Log or handle the exception
                        ModelState.AddModelError("", $"Request error: {ex.Message}");
                        return View("Login", "Login");
                    }
                }
            }

            return View("Login", user);
        }

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]  //i thhought this would be httpget but httppost works and get throws an error
        //public IActionResult Login(LoginModel model)
        //{
        //    bool accountExists = CheckLogin(model.Username, model.Password);

        //    if (ModelState.IsValid)
        //    {
        //        if (accountExists)  //change to where admin and password are in database table, and instead of model.Username, model.Password u are taking from the broker sign in object
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ModelState.AddModelError("", "Invalid username or password.");
        //    }
        //    return View(model);
        //}



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

/*
using Microsoft.AspNetCore.Mvc;
using HomeEstate.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace HomeEstate.Controllers
{
    public class LoginController : Controller
    {
        // API base URL
        private readonly Uri webApiUrl = new Uri("http://localhost:7229/api");

        [HttpPost]
        public async Task<IActionResult> CheckLogin(LoginModel user)
        {
            // Ensure the model is valid
            if (!ModelState.IsValid)
            {
                return View("Login"); // Return to login view if validation fails
            }

            var jsonLogin = JsonSerializer.Serialize(user);
            var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    // Post the login data to the API
                    var response = await client.PostAsync(webApiUrl + "/Login/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response
                        var data = await response.Content.ReadAsStringAsync();
                        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(data);

                        if (loginResponse?.Success == true)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", loginResponse?.Message ?? "Invalid credentials.");
                            return View("Login"); // Return to login view with error message
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error occurred while checking credentials.");
                        return View("Login");
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Log or handle the exception
                    ModelState.AddModelError("", $"Request error: {ex.Message}");
                    return View("Login");
                }
            }
        }

        // Login GET action
        public IActionResult Login()
        {
            return View();
        }

        // Other actions (Register, ForgetPassword, RestartPassword, etc.) are omitted for brevity
    }

    // Define a class to deserialize the login response from the Web API
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
*/