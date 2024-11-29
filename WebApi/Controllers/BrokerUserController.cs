﻿using Microsoft.AspNetCore.Mvc;
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
            SqlParameter inputBrokerID = new SqlParameter("@BrokerId", id);
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
        [HttpGet("GetOfferByBroker/{id}")]
        public List<GetHomeOfferModel> GetOfferByBroker(int id)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetHomeOffer";
            SqlParameter inputBrokerID = new SqlParameter("@BrokerId", id);
            inputBrokerID.Direction = ParameterDirection.Input;
            objCommand.Parameters.Add(inputBrokerID);

            DataSet ds = objDB.GetDataSet(objCommand);
            List<GetHomeOfferModel> offer=new List<GetHomeOfferModel>();



            foreach (DataRow row in ds.Tables[0].Rows)
            {
                offer.Add(new GetHomeOfferModel
                {
                    HomeId = Convert.ToInt32(row["HomeId"]),
                    Address = row["Address"].ToString(),
                    AskingPrice = Convert.ToInt32(row["AskingPrice"]),
                    OfferName = row["OfferName"].ToString(),
                    OfferEmail = row["OfferEmail"].ToString(),
                    OfferPhone = row["OfferPhone"].ToString(),
                    OfferAmount = Convert.ToInt32(row["OfferAmount"]),
                    SaleType = row["SaleType"].ToString(),
                    Contingencies = row["Contingencies"].ToString(),
                    NeedsToSellHome = row["NeedsToSellHome"].ToString(),
                    PreferredMoveInDate = row["PreferredMoveInDate"].ToString()
                });
            }


            return offer;

        }
        [HttpGet("GetShowingByBroker/{id}")]
        public List<GetHomeShowingModel> GetShowingByBroker(int id)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetShowingsByBroker";
            SqlParameter inputBrokerID = new SqlParameter("@BrokerId", id);
            inputBrokerID.Direction = ParameterDirection.Input;
            objCommand.Parameters.Add(inputBrokerID);

            DataSet ds = objDB.GetDataSet(objCommand);
            List<GetHomeShowingModel> showings = new List<GetHomeShowingModel>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                showings.Add(new GetHomeShowingModel
                {
                    ShowingId = Convert.ToInt32(row["ShowingId"]),
                    BuyerName = row["BuyerName"].ToString(),
                    BuyerEmail = row["BuyerEmail"].ToString(),
                    BuyerPhone = row["BuyerPhone"].ToString(),
                    ShowingDate = row["ShowingDate"].ToString(),
                    HomeId = Convert.ToInt32(row["HomeId"]),
                    Address = row["Address"].ToString(),
                    PropertyType = row["PropertyType"].ToString(),
                    AskingPrice = Convert.ToInt32(row["AskingPrice"])
                });
            }

            return showings;
        }
        [HttpDelete("DeleteHome")]
        public bool DeleteHome(int id)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "DeleteHouse";
            SqlParameter Deletehouse = new SqlParameter("@HomeID", id);
            objCommand.Parameters.Add(Deletehouse);
            objDB.DoUpdate(objCommand);
            return true;
        }
    }
}