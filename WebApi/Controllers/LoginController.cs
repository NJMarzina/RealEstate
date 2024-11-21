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
    }
}
