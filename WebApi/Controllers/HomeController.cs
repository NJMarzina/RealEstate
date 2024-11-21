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

    }
}
