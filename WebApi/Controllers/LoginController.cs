using Microsoft.AspNetCore.Mvc;
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

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        [HttpPost()]
        [HttpPost("CheckLogin")]
        public bool CheckLogin([FromBody] LoginModel broker)
        {
            bool isCorrect = false;

            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "BrokerLogin";

            objCommand.Parameters.AddWithValue("@UserName", broker.Username);
            objCommand.Parameters.AddWithValue("@UserPassword", broker.Password);

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

        [HttpGet("GetBrokerIDByUsername/{username}")]  // GET api/CustomerService/GetCustomerByName/
        public int GetBrokerIDByUsername(String username)
        {
            int brokerID = 0;
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetBrokerIDByUsername";   //create this stored procedure that gets the broker id for the username thats logging in (or find it)
            objCommand.Parameters.AddWithValue("@username", username);
            DataSet myDS = objDB.GetDataSetUsingCmdObj(objCommand);

            /*
            if (myDS.Tables[0].Rows.Count > 0)
            {
                cust = new Customer();
                cust.Name = objDB.GetField("CustomerName", 0).ToString();
                cust.Address = objDB.GetField("Address", 0).ToString();
                cust.City = objDB.GetField("City", 0).ToString();
                cust.State = objDB.GetField("State", 0).ToString();
                cust.Zip = objDB.GetField("Zip", 0).ToString();
                cust.Email = objDB.GetField("Email", 0).ToString();
            }
            */

            if (myDS.Tables[0].Rows.Count > 0)
            {
                brokerID = int.Parse(objDB.GetField("BrokerID", 0).ToString());
            }

            return brokerID;

        }
    }
}
