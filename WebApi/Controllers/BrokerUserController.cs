using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Models;
using WebApi.Utilities;
using WebApi.Utilities.HomeEstate.Utilities;
using HomeLibrary;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;

using System.Security.Cryptography;     // needed for the encryption classes
using System.IO;                        // needed for the MemoryStream
using System.Text;                      // needed for the UTF8 encoding
using System.Net;
using static Azure.Core.HttpHeader;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerUserController : Controller
    {
            [HttpGet("GetHomeByBroker/{id}")]
            public List<HomeModel> GetHomeByBroker(int id)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetHomesByBroker";
            SqlParameter inputBrokerID = new SqlParameter("@BrokerId",id);
            inputBrokerID.Direction = ParameterDirection.Input;
            objCommand.Parameters.Add(inputBrokerID);
            DataSet ds = objDB.GetDataSet(objCommand);

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
                        Size = row["Size"] != DBNull.Value ? Convert.ToSingle(row["Size"]) : 0.0f, // Changed to float (Single in C#)                        Heating = row["Heating"].ToString(),
                        Heating = row["Heating"].ToString(),
                        Cooling = row["Cooling"].ToString(),
                        YearBuild = Convert.ToInt32(row["Year_Build"]),
                        Garage = row["Garage"].ToString(),
                        Utilities = row["Utilities"].ToString(),
                        Description = row["Description"].ToString(),
                        AskingPrice = Convert.ToInt32(row["AskingPrice"]),
                        Status = row["Status"].ToString()
                    });
                }

                return homes;
            }

        
    }
}