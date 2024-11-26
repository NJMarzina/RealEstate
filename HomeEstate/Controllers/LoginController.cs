using Microsoft.AspNetCore.Mvc;
using HomeEstate.Models;
using System.Net;
using System.Text.Json;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;

using Nancy.Json;
using System.IO;                        // needed for Stream and Stream Reader
using System.Net;                       // needed for the Web Request
using System.Data;                      // needed for DataSet class

using Newtonsoft.Json;

using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Converters;
using HomeLibrary;
//using RealEstate.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Security.Cryptography;


namespace HomeEstate.Controllers
{
    public class LoginController : Controller
    {
        private Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
        private Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

        Uri address = new Uri("https://cis-iis2.temple.edu/Fall2024/cis3342_tun52511/TermProject/api/Login/");
        string webApiUrl = "http://localhost:7285/api/Login/";

        [HttpPost]
        public IActionResult CheckLogin(LoginModel user)
        {
            //String webApiUrl = "https://localhost:7229/api/Login/CheckLogin";

            string username = user.Username;
            //string encryptedPassword = user.Password;

            //password encryption process
            string plainTextPassword = user.Password;
            string encryptedPassword;

            UTF8Encoding encoder = new UTF8Encoding();      // used to convert bytes to characters, and back
            Byte[] textBytes;                               // stores the plain text data as bytes

            textBytes = encoder.GetBytes(plainTextPassword);

            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

            myEncryptionStream.Write(textBytes, 0, textBytes.Length);
            myEncryptionStream.FlushFinalBlock();

            myMemoryStream.Position = 0;
            Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

            myEncryptionStream.Close();
            myMemoryStream.Close();

            encryptedPassword = Convert.ToBase64String(encryptedBytes);

            user.Password = encryptedPassword;
            //end of encryption

            JavaScriptSerializer js = new JavaScriptSerializer();
            var jsonUser = js.Serialize(user);
            //try
            //{
                // Send the Customer object to the Web API that will be used to store a new customer record in the database.
                // Setup an HTTP POST Web Request and get the HTTP Web Response from the server.
                WebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:7285/api/Login/CheckLogin");    //was webApiUrl + "/CheckLogin"
                request.Method = "POST";
                request.ContentLength = jsonUser.Length;
                request.ContentType = "application/json";
                // Write the JSON data to the Web Request
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonUser);
                writer.Flush();
                writer.Close();
                // Read the data from the Web Response, which requires working with streams.
                WebResponse response = (HttpWebResponse)request.GetResponse();
                Stream theDataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(theDataStream);
                string data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            //}
            //catch (Exception ex)
            //{ 
            //    return View();
            //}
            if (data=="true")
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                };

                Response.Cookies.Append("Username", user.Username, cookieOptions);
                Response.Cookies.Append("Password", user.Password, cookieOptions);

                webApiUrl = "https://localhost:7285/api/Login/GetBrokerIDByUsername/";
                //WebRequest request = WebRequest.Create(webApiUrl + "GetCustomerByName/" + txtName.Text);
                HttpWebRequest requestBrokerID = (HttpWebRequest)WebRequest.Create(webApiUrl + user.Username);
                requestBrokerID.Method = "GET";
                requestBrokerID.ContentType = "application/json";
                HttpWebResponse responseBrokerID = (HttpWebResponse)requestBrokerID.GetResponse();

                Stream theBrokerIDDataStream = responseBrokerID.GetResponseStream();
                StreamReader readerBrokerID = new StreamReader(theBrokerIDDataStream);
                string brokerIDData = readerBrokerID.ReadToEnd();
                readerBrokerID.Close();
                responseBrokerID.Close();
                JavaScriptSerializer jsBrokerID = new JavaScriptSerializer();
                int brokerID = jsBrokerID.Deserialize<int>(brokerIDData);

                Response.Cookies.Append("BrokerID", brokerID.ToString(), cookieOptions);
                Response.Cookies.Append("Username", user.Username, cookieOptions);

                return RedirectToAction("BrokerDashboard", "Broker");
                //return View("~/Views/Broker/BrokerDashboard.cshtml");
                //return View("Dashboard");
            }
            return View("Login");
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
        public IActionResult Registers(RegisterModel model)
        {
            RegisterModel Broker = new RegisterModel();
            Broker.UserName = model.UserName;
            Broker.UserPassword = model.UserPassword;
            Broker.FullName = model.FullName;
            Broker.HomeEmail = model.HomeEmail;
            Broker.AddressName = model.AddressName;
            Broker.AddressNumber = model.AddressNumber;
            Broker.WorkAddressNumber = model.WorkAddressNumber;
            Broker.WorkAddressName=model.WorkAddressName;
            Broker.WorkEmail=model.WorkEmail;
            Broker.RealEstateCompany = model.RealEstateCompany;
            Broker.CompanyPhone=model.CompanyPhone;
           
            Broker.SecurityQuestion1=model.SecurityQuestion1;
            Broker.SecurityAnswer1 = model.SecurityAnswer1;
            Broker.SecurityQuestion2 = model.SecurityQuestion2;
            Broker.SecurityAnswer2 = model.SecurityAnswer2;
            Broker.SecurityQuestion3 = model.SecurityQuestion3;
            Broker.SecurityAnswer3= model.SecurityAnswer3;

            JavaScriptSerializer js = new JavaScriptSerializer();
            string ApiUrl = "https://localhost:7285/api/Home/AddBroker";
            string jsonCustomer = js.Serialize(Broker);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            request.Method = "POST";

            request.ContentType = "application/json";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonCustomer);
            writer.Flush();
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream theDataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(theDataStream);

            string data = reader.ReadToEnd();

            reader.Close();

            response.Close();
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