using Microsoft.AspNetCore.Mvc;
using HomeEstate.Models;
using System.Net;
using System.Text.Json;

namespace HomeEstate.Controllers
{
    public class LoginController : Controller
    {
        Uri address = new Uri("https://cis-iis2.temple.edu/Fall2024/cis3342_tun52511/TermProject/api");
        Uri webApiUrl = new Uri("http://localhost:7229/api");



        [HttpPost]
        public IActionResult CheckLogin(LoginModel User)
        {
            LoginModel NewUser = new LoginModel();
            User.Username = Request.Form["Username"];
            User.Password = Request.Form["Password"];

            if (ModelState.IsValid)
            {
                
                //bool rememberMe = Request.Form["chkRememberme"] == "on";

                var jsonLogin = JsonSerializer.Serialize(NewUser);

                try
                {
                    WebRequest request = WebRequest.Create(webApiUrl + "CheckLogin/");
                    request.Method = "POST";
                    request.ContentLength = jsonLogin.Length;
                    request.ContentType = "application/json";

                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(jsonLogin);
                    writer.Flush();
                    writer.Close();

                    WebResponse response = request.GetResponse();
                    Stream theDataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(theDataStream);
                    String data = reader.ReadToEnd();
                    reader.Close();
                    response.Close();

                    /*
                    if (rememberMe)
                    {
                        var cookieOptions = new CookieOptions
                        {

                            Expires = DateTime.Now.AddDays(30),
                        };


                        Response.Cookies.Append("Username", login.Username, cookieOptions);
                        Response.Cookies.Append("Password", login.Password, cookieOptions);
                    }
                    

                    if (data == "true")
                    {
                        var cookieOptions = new CookieOptions
                        {

                            Expires = DateTime.Now.AddDays(30),
                        };


                        Response.Cookies.Append("UsernameSession", login.Username, cookieOptions);
                        return RedirectToAction("GetAllProfiles", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("LogIn", "Home");
                    }
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            */
                    //else
                    //{
                return View(NewUser);
                    //}
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(NewUser);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]  //i thhought this would be httpget but httppost works and get throws an error
        public IActionResult Login(LoginModel model)
        {
            bool accountExists = LoginModel.CheckLogin(model.Username, model.Password);

            if (ModelState.IsValid)
            {
                if (accountExists)  //change to where admin and password are in database table, and instead of model.Username, model.Password u are taking from the broker sign in object
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
