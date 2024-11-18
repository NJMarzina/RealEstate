using Azure;
using HomeEstate.Utilities;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Common;

namespace HomeEstate.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public static bool CheckLogin(string username, string password)
        {
            bool isCorrect = false;

           DBConnect objDB = new DBConnect();
           SqlCommand objCommand = new SqlCommand();
            /*
           command.CommandType = CommandType.StoredProcedure;
           command.CommandText = "CheckLoginByUsernameAndPassword";

           command.Parameters.AddWithValue("@Username", username);
           command.Parameters.AddWithValue("@Password", password);

           var result = objDB.DoUpdateUsingCmdObj(command);

           if (result == 1)
           {
               isCorrect = true;
           }

           return isCorrect;
            */

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
