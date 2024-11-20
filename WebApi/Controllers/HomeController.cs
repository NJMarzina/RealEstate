using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Utilities;
using WebApi.Utilities.HomeEstate.Utilities;



namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
   
        [HttpGet("GetHomeData")]
        public IActionResult GetHomeData()
        {
            
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();
                
                   
                   String SQL= "SELECT Address_Number, Address_Name, AddressCity, AddressState, AddressZip, Property_Type, Heating, Cooling, Year_Build, Garage, Utilities, Description, AskingPrice, Status FROM Home";
               

                // Execute the query and fetch the data
                DataSet ds = objDB.GetDataSet(SQL);

                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);

                return Ok(jsonResult);
            
           
        }
    }
}
