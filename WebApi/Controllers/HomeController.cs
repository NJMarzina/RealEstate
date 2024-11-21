using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Models;
using WebApi.Utilities;
using WebApi.Utilities.HomeEstate.Utilities;



namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("GetHomeData")]
        public List<HomeModel> GetHomeData()
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommandProfile = new SqlCommand();

            string SQL = "SELECT Address_Number, Address_Name, AddressCity, AddressState, AddressZip, Property_Type ,Year_Build, AskingPrice, Status FROM Home";
            DataSet ds = objDB.GetDataSet(SQL);

            List<HomeModel> homes = new List<HomeModel>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                homes.Add(new HomeModel
                {
                    AddressNumber = row["Address_Number"].ToString(),
                    AddressName = row["Address_Name"].ToString(),
                    AddressCity = row["AddressCity"].ToString(),
                    AddressState = row["AddressState"].ToString(),
                    AddressZip = row["AddressZip"].ToString(),
                    PropertyType = row["Property_Type"].ToString(),             
                    YearBuild = Convert.ToInt32(row["Year_Build"]),                
                    AskingPrice = Convert.ToDecimal(row["AskingPrice"]), 
                });
            }

            return homes;
        }

        [HttpPost("AddBroker")]
        public bool AddBroker([FromBody] BrokerModel broker)
        {
            String Broker = "1";
            if (broker != null)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommandProfile = new SqlCommand();

                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "RegisterBroker";

                SqlParameter parameter = new SqlParameter("returnValue", SqlDbType.Int);
                parameter.Direction = ParameterDirection.ReturnValue;
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@Username", broker.UserName);
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@UserPassword", broker.UserPassword);
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@FullName", broker.FullName);
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@HomeEmail", broker.HomeEmail);
                objCommand.Parameters.Add(parameter);
                parameter = new SqlParameter("@AddressName", broker.AddressName);
                objCommand.Parameters.Add(parameter);
                parameter = new SqlParameter("@AddressNumber", broker.AddressNumber);
                objCommand.Parameters.Add(parameter);
                
                
                int retVal = objDB.DoUpdate(objCommand);
               
                broker.BrokerId = Convert.ToInt32(objCommand.Parameters["returnValue"].Value.ToString());
                objCommandProfile.CommandType = CommandType.StoredProcedure;
                objCommandProfile.CommandText = "AddBrokerID";
                SqlParameter profileParameter = new SqlParameter("@BrokerId", broker.BrokerId);
                objCommandProfile.Parameters.Add(profileParameter);
                objDB.DoUpdate(objCommandProfile);
                if (retVal > 0)
                    return true;
                else
                    return false;
            }
            else

            {
                return false;
            }

        }


    }
}
