using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Utilities;
using WebApi.Utilities.HomeEstate.Utilities;


namespace WebApi.Controllers
{
    public class LoginController : Controller
    {
        [Produces("application/json")]
        [Route("api/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("CheckLogin")]
        public static bool CheckLogin(string username, string password)
        {
            bool isCorrect = false;

            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "BrokerLogin";

            objCommand.Parameters.AddWithValue("@UserName", username);
            objCommand.Parameters.AddWithValue("@UserPassword", password);

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

            string id = objCommand.Parameters["@BrokerId"].Value.ToString();
            string name = objCommand.Parameters["@FullName"].Value.ToString();


            if (id == "")
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
