using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Models;
using WebApi.Utilities;
using WebApi.Utilities.HomeEstate.Utilities;
using HomeLibrary;


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

            string SQL = "SELECT Home_ID,Address_Number, Address_Name, AddressCity, AddressState, AddressZip, Property_Type ,Year_Build, AskingPrice, Status FROM Home";
            DataSet ds = objDB.GetDataSet(SQL);

            List<HomeModel> homes = new List<HomeModel>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                homes.Add(new HomeModel
                {
                    homeId = Convert.ToInt32(row["Home_ID"].ToString()),
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
        public bool AddBroker([FromBody]  BrokerProfile Profile)
        {
            String Broker = "1";
            if (Profile != null)
            {
                DBConnect objDB = new DBConnect();              
                SqlCommand objCommandProfile = new SqlCommand();

                DBConnect ProfileDB = new DBConnect();
                SqlCommand objProfileUpdate = new SqlCommand();


                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "RegisterBroker";

                SqlParameter parameter = new SqlParameter("returnValue", SqlDbType.Int);
                parameter.Direction = ParameterDirection.ReturnValue;
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@Username", Profile.UserName);
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@UserPassword", Profile.UserPassword);
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@FullName", Profile.FullName);
                objCommand.Parameters.Add(parameter);

                parameter = new SqlParameter("@HomeEmail", Profile.HomeEmail);
                objCommand.Parameters.Add(parameter);
                parameter = new SqlParameter("@AddressName", Profile.AddressName);
                objCommand.Parameters.Add(parameter);
                parameter = new SqlParameter("@AddressNumber", Profile.AddressNumber);
                objCommand.Parameters.Add(parameter);
                
                
                int retVal = objDB.DoUpdate(objCommand);

                Profile.BrokerId = Convert.ToInt32(objCommand.Parameters["returnValue"].Value.ToString());
                objCommandProfile.CommandType = CommandType.StoredProcedure;
                objCommandProfile.CommandText = "AddBrokerID";
                SqlParameter profileParameter = new SqlParameter("@BrokerId", Profile.BrokerId);
                objCommandProfile.Parameters.Add(profileParameter);
                objDB.DoUpdate(objCommandProfile);


                objProfileUpdate.CommandText = "UpdateBrokerProfile";
                objProfileUpdate.CommandType = CommandType.StoredProcedure;

                SqlParameter updateparameter = new SqlParameter("@BrokerId", Profile.BrokerId);
                objProfileUpdate.Parameters.Add(updateparameter);

                updateparameter = new SqlParameter("@WorkAddressName", Profile.WorkAddressName);
                objProfileUpdate.Parameters.Add(updateparameter);

                updateparameter = new SqlParameter("@WorkAddressNumber", Profile.WorkAddressNumber);
                objProfileUpdate.Parameters.Add(updateparameter);


                updateparameter = new SqlParameter("@WorkEmail",Profile.WorkEmail);
                objProfileUpdate.Parameters.Add(updateparameter);

                updateparameter = new SqlParameter("@RealEstateCompany",Profile.RealEstateCompany);
                objProfileUpdate.Parameters.Add(updateparameter);


                updateparameter = new SqlParameter("@CompanyPhone", Profile.CompanyPhone);
                objProfileUpdate.Parameters.Add(updateparameter);

                ProfileDB.DoUpdate(objProfileUpdate);


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
