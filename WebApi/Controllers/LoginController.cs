﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using WebApi.Models;
using WebApi.Utilities;
using WebApi.Utilities.HomeEstate.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

using System.Security.Cryptography;     // needed for the encryption classes
using System.IO;                        // needed for the MemoryStream
using System.Text;                      // needed for the UTF8 encoding
using System.Net;                       // needed for the cookie

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
        private Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

        [HttpPost()]
        [HttpPost("CheckLogin")]
        public bool CheckLogin([FromBody] LoginModel broker)
        {
            //get password for attempted username
            //decrypt password associated
            //if decrypted matches attempted, approve
            //stored procedure called GetEncryptedPasswordByUsername

            //we can keep all this code if we just change "objCommand.Parameters.AddWithValue("@UserPassword", broker.Password);" to decrypted not broker.Password
            bool isCorrect = false;

            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "BrokerLogin";

            objCommand.Parameters.AddWithValue("@UserName", broker.Username);
            objCommand.Parameters.AddWithValue("@UserPassword", broker.Password);   //will maintain code, but change this to decrpyted password not broker.Password

            SqlParameter userIdParameter = new SqlParameter();
            userIdParameter.ParameterName = "@BrokerId";
            userIdParameter.SqlDbType = SqlDbType.Int;
            userIdParameter.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(userIdParameter);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@FullName";
            nameParameter.SqlDbType = SqlDbType.VarChar;
            nameParameter.Size = 50;
            nameParameter.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(nameParameter);

            objDB.DoUpdateUsingCmdObj(objCommand);

            string id = "";
            string name = "";

            id = objCommand.Parameters["@BrokerId"].Value?.ToString();
            name = objCommand.Parameters["@FullName"].Value?.ToString();

            //name = "Jason";


            if (string.IsNullOrEmpty(id))
            {
                isCorrect = false;
            }
            else
            {
                isCorrect = true;
            }

            return isCorrect;
        }

        [HttpGet("GetQuestion/{Userid}")]
        public SecurityQuestions GetQuestions(int Userid)
        {
            SecurityQuestions securityQuestions = new SecurityQuestions();

            try
            {
                // Initialize database connection
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetSecurityQuestion"
                };

                // Add UserID as a parameter
                objCommand.Parameters.AddWithValue("@id", Userid);

                // Execute the command and get the dataset
                DataSet ds = objDB.GetDataSet(objCommand);

                // Check if the dataset contains rows
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // Assign the first question and answer from the dataset to the model
                    securityQuestions.Question = ds.Tables[0].Rows[0]["Question"].ToString();
                    securityQuestions.Answer = ds.Tables[0].Rows[0]["Answer"].ToString(); 
                }
            }
            catch (Exception ex)
            {
            }

            return securityQuestions;
        }


        [HttpGet("GetBrokerIDByUsername/{username}")]  // GET api/CustomerService/GetCustomerByName/
        public int GetBrokerIDByUsername(String username)
        {
            int brokerID = 0;
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetBrokerIDByUsername";
            objCommand.Parameters.AddWithValue("@username", username);
            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            if (myDS.Tables[0].Rows.Count > 0)
            {
                brokerID = int.Parse(objDB.GetField("BrokerId", 0).ToString());
            }

            return brokerID;

        }

        [HttpGet("GetProfileIDByBrokerID/{brokerID}")]
        public int GetProfileIDByBrokerID(int brokerID)
        {
            int profileID = 0;
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetProfileIDByBrokerID";  //make
            objCommand.Parameters.AddWithValue("@brokerID", brokerID);
            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            if (myDS.Tables[0].Rows.Count > 0)
            {
                profileID = int.Parse(objDB.GetField("ProfileId", 0).ToString());
            }

            return profileID;
        }
    }
}
