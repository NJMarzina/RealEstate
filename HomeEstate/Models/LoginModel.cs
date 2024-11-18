using HomeEstate.Utilities;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
            DBConnect objDB = new DBConnect();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CheckLoginByUsernameAndPassword";

            //command.Parameters.AddWithValue("@username", theCar.VIN);
            //command.Parameters.AddWithValue("@password", theCar.Make);

            return true;
        }
    }
}
