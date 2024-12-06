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
using HomeEstate.Utilities;

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
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


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
              
                WebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:7285/api/Login/CheckLogin");    //was webApiUrl + "/CheckLogin"
                request.Method = "POST";
                request.ContentLength = jsonUser.Length;
                request.ContentType = "application/json";

                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonUser);
                writer.Flush();
                writer.Close();

                WebResponse response = (HttpWebResponse)request.GetResponse();
                Stream theDataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(theDataStream);
                string data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            if (data=="true")
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                };

                //Response.Cookies.Append("Username", user.Username, cookieOptions);
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

                //getting profileID
                webApiUrl = "https://localhost:7285/api/Login/GetProfileIDByBrokerID/";

                HttpWebRequest requestProfileID = (HttpWebRequest)WebRequest.Create(webApiUrl + brokerID.ToString());
                requestProfileID.Method = "GET";
                requestProfileID.ContentType = "application/json";
                HttpWebResponse responseProfileID = (HttpWebResponse)requestProfileID.GetResponse();

                Stream theProfileIDDataStream = responseProfileID.GetResponseStream();
                StreamReader readerProfileID = new StreamReader(theProfileIDDataStream);
                string profileIDData = readerProfileID.ReadToEnd();
                readerProfileID.Close();
                responseProfileID.Close();
                JavaScriptSerializer jsProfileID = new JavaScriptSerializer();
                int profileID = jsProfileID.Deserialize<int>(profileIDData);

                Response.Cookies.Append("ProfileID", profileID.ToString(), cookieOptions);

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
            int test = 0;

                try
                {
                    // Get broker ID based on username
                    string webApiUrl = "https://localhost:7285/api/Login/GetBrokerIDByUsername/";
                    HttpWebRequest requestBrokerID = (HttpWebRequest)WebRequest.Create(webApiUrl + model.UserName);
                    requestBrokerID.Method = "GET";
                    requestBrokerID.ContentType = "application/json";

                    using (HttpWebResponse responseBrokerID = (HttpWebResponse)requestBrokerID.GetResponse())
                    using (Stream theBrokerIDDataStream = responseBrokerID.GetResponseStream())
                    using (StreamReader readerBrokerID = new StreamReader(theBrokerIDDataStream))
                    {
                        string brokerIDData = readerBrokerID.ReadToEnd();
                        JavaScriptSerializer jsBrokerID = new JavaScriptSerializer();
                        int brokerID = jsBrokerID.Deserialize<int>(brokerIDData);

                        if (brokerID > 0)
                        {
                            TempData["BrokerID"] = brokerID;
                            TempData["UserName"] = model.UserName;

                            return RedirectToAction("SecurityQuestion", "Login");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username not found.");
                            return View(model);
                        }
                    }
                }
                catch (WebException ex)
                {
                    return View(model);
                }

            return View(model);
        }

        [HttpGet]
        public IActionResult SecurityQuestion()
        {
            if (TempData["BrokerID"] == null || TempData["UserName"] == null)
            {
                return RedirectToAction("ForgetPassword");
            }

            try
            {
                int brokerId = Convert.ToInt32(TempData["BrokerID"]);
                string userName = TempData["UserName"].ToString();

                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetSecurityQuestion";

                objCommand.Parameters.AddWithValue("@id", brokerId);

                SqlParameter questionParameter = new SqlParameter("@Question", SqlDbType.VarChar, 500);
                questionParameter.Direction = ParameterDirection.Output;
                objCommand.Parameters.Add(questionParameter);

                SqlParameter answerParameter = new SqlParameter("@Answer", SqlDbType.VarChar, 500);
                answerParameter.Direction = ParameterDirection.Output;
                objCommand.Parameters.Add(answerParameter);

                objDB.DoUpdateUsingCmdObj(objCommand);

                string question = objCommand.Parameters["@Question"].Value.ToString();
                string answer = objCommand.Parameters["@Answer"].Value.ToString();

                if (!string.IsNullOrEmpty(question))
                {
                    var model = new ResetPasswordModel
                    {
                        UserName = userName,
                        Question = question
                    };

                    TempData["Question"] = question;
                    TempData["CorrectAnswer"] = answer;
                    TempData["BrokerID"] = TempData["BrokerID"];
                    TempData["UserName"] = TempData["UserName"];

                    ViewData["Question"] = question;

                    return View(model);
                }
                else
                {
                    return RedirectToAction("ForgetPassword");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ForgetPassword");
            }
        }

        [HttpPost]
        public IActionResult SecurityQuestion(ResetPasswordModel model)
        {
            int brokerId = int.Parse(TempData["BrokerID"].ToString());
            string correctAnswer = TempData["CorrectAnswer"].ToString();
            string userName = TempData["UserName"].ToString();

            if (string.Equals(model.Answer, correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "ValidateSecurityAnswer";
                objCommand.Parameters.AddWithValue("@BrokerId", brokerId);
                objCommand.Parameters.AddWithValue("@Answer", model.Answer);

                SqlParameter isValidParameter = new SqlParameter();
                isValidParameter.ParameterName = "@IsValid";
                isValidParameter.SqlDbType = SqlDbType.Bit;
                isValidParameter.Direction = ParameterDirection.Output;
                objCommand.Parameters.Add(isValidParameter);

                objDB.DoUpdateUsingCmdObj(objCommand);

                bool isValid = Convert.ToBoolean(objCommand.Parameters["@IsValid"].Value);

                if (isValid)
                {
                    return RedirectToAction("RestartPassword", new { username = userName });
                }
            }

            ModelState.AddModelError("Answer", "Incorrect security answer.");
            model.Question = TempData["Question"].ToString();
            return View(model);
        }

        [HttpGet]
        public IActionResult RestartPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "No username provided.");
                return RedirectToAction("ForgetPassword");
            }

            ViewData["Username"] = username;

            return View();
        }

        [HttpPost]
        public IActionResult RestartPassword(ResetPasswordModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and confirmation password do not match.");
                return View(model);
            }

            string username = model.UserName;
            string password = model.NewPassword;


            string encryptedPassword;

            UTF8Encoding encoder = new UTF8Encoding();      // used to convert bytes to characters, and back
            Byte[] textBytes;                               // stores the plain text data as bytes

            textBytes = encoder.GetBytes(password);

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

            try
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "UpdatePassword";

                objCommand.Parameters.AddWithValue("@Username", username);
                objCommand.Parameters.AddWithValue("@NewPassword", encryptedPassword);

                objDB.DoUpdateUsingCmdObj(objCommand);

                TempData["PasswordResetSuccess"] = "Your password has been successfully reset.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        /*

        [HttpPost]
        public IActionResult RestartPassword(string username)
        {
            // Pass the username to the RestartPassword view (if needed)
            ViewData["Username"] = username;
            return View();
        }
        */

    }
}