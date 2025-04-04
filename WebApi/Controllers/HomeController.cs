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
using System.Net;                       // needed for the cookie


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
        private Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

        [HttpGet("GetHomeData")]
        public List<HomeModel> GetHomeData()
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommandProfile = new SqlCommand();


            string SQL = @"
    SELECT 
        h.Home_ID,
        h.Address_Number, 
        h.Address_Name, 
        h.AddressCity, 
        h.AddressState, 
        h.AddressZip, 
        h.Property_Type,
        h.Year_Build, 
        h.AskingPrice, 
        h.Status,
        (SELECT TOP 1 hi.Imagie 
         FROM HomeImage hi 
         WHERE hi.Home_Id = h.Home_ID) AS Imagie
    FROM 
        Home h";


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
                    AskingPrice = Convert.ToInt32(row["AskingPrice"]),
                    ImageUrl = row["Imagie"] == DBNull.Value ? null : row["Imagie"].ToString() 

                });
            }

            return homes;
        }

        [HttpGet("SearchHome")]

        public List<HomeModel> SearchHome(string city)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetHomesByCity";
            objCommand.Parameters.AddWithValue("@City", city);
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
                    YearBuild = Convert.ToInt32(row["Year_Build"]),
                    AskingPrice = Convert.ToInt32(row["AskingPrice"]),
                    ImageUrl = row["Imagie"] == DBNull.Value ? null : row["Imagie"].ToString()

                });
            }

            return homes;

        }




        [HttpGet("GetHomeDetails/{id}")]
        public HomeDetails GetHomeDetails(int id)
        {
            HomeDetails home = null;
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetAllDetails";
            objCommand.Parameters.AddWithValue("@HomeID", id);
            SqlParameter AddressNumber = new SqlParameter();
            AddressNumber.ParameterName = "@AddressNumber";
            AddressNumber.SqlDbType = SqlDbType.VarChar;
            AddressNumber.Size = 50;
            AddressNumber.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(AddressNumber);

            SqlParameter AddressName = new SqlParameter();
            AddressName.ParameterName = "@AddressName";
            AddressName.SqlDbType = SqlDbType.VarChar;
            AddressName.Size = 255;
            AddressName.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(AddressName);

            SqlParameter AddressCity = new SqlParameter();
            AddressCity.ParameterName = "@AddressCity";
            AddressCity.SqlDbType = SqlDbType.VarChar;
            AddressCity.Size = 50;
            AddressCity.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(AddressCity);

            SqlParameter AddressState = new SqlParameter();
            AddressState.ParameterName = "@AddressState";
            AddressState.SqlDbType = SqlDbType.VarChar;
            AddressState.Size = 10;
            AddressState.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(AddressState);

            SqlParameter AddressZip = new SqlParameter();
            AddressZip.ParameterName = "@AddressZip";
            AddressZip.SqlDbType = SqlDbType.VarChar;
            AddressZip.Size = 50;
            AddressZip.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(AddressZip);

            SqlParameter PropertyType = new SqlParameter();
            PropertyType.ParameterName = "@PropertyType";
            PropertyType.SqlDbType = SqlDbType.VarChar;
            PropertyType.Size = 100;
            PropertyType.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(PropertyType);

            SqlParameter Heating = new SqlParameter();
            Heating.ParameterName = "@Heating";
            Heating.SqlDbType = SqlDbType.VarChar;
            Heating.Size = 100;
            Heating.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(Heating);

            SqlParameter Cooling = new SqlParameter();
            Cooling.ParameterName = "@Cooling";
            Cooling.SqlDbType = SqlDbType.VarChar;
            Cooling.Size = 100;
            Cooling.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(Cooling);

            SqlParameter YearBuild = new SqlParameter();
            YearBuild.ParameterName = "@YearBuild";
            YearBuild.SqlDbType = SqlDbType.Int;
            YearBuild.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(YearBuild);

            SqlParameter Garage = new SqlParameter();
            Garage.ParameterName = "@Garage";
            Garage.SqlDbType = SqlDbType.VarChar;
            Garage.Size = 50;
            Garage.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(Garage);

            SqlParameter Utilities = new SqlParameter();
            Utilities.ParameterName = "@Utilities";
            Utilities.SqlDbType = SqlDbType.VarChar;
            Utilities.Size = 255;
            Utilities.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(Utilities);

            SqlParameter AskingPrice = new SqlParameter();
            AskingPrice.ParameterName = "@AskingPrice";
            AskingPrice.SqlDbType = SqlDbType.Int;
            AskingPrice.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(AskingPrice);

            SqlParameter WorkAddressName = new SqlParameter();
            WorkAddressName.ParameterName = "@WorkAddressName";
            WorkAddressName.SqlDbType = SqlDbType.VarChar;
            WorkAddressName.Size = 50;
            WorkAddressName.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(WorkAddressName);

            SqlParameter WorkAddressNumber = new SqlParameter();
            WorkAddressNumber.ParameterName = "@WorkAddressNumber";
            WorkAddressNumber.SqlDbType = SqlDbType.VarChar;
            WorkAddressNumber.Size = 50;
            WorkAddressNumber.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(WorkAddressNumber);

            SqlParameter WorkEmail = new SqlParameter();
            WorkEmail.ParameterName = "@WorkEmail";
            WorkEmail.SqlDbType = SqlDbType.VarChar;
            WorkEmail.Size = 50;
            WorkEmail.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(WorkEmail);
            SqlParameter RealEstateCompany = new SqlParameter();
            RealEstateCompany.ParameterName = "@RealEstateCompany";
            RealEstateCompany.SqlDbType = SqlDbType.VarChar;
            RealEstateCompany.Size = 50;
            RealEstateCompany.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(RealEstateCompany);
            SqlParameter CompanyPhone = new SqlParameter();
            CompanyPhone.ParameterName = "@CompanyPhone";
            CompanyPhone.SqlDbType = SqlDbType.VarChar;
            CompanyPhone.Size = 50;
            CompanyPhone.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(CompanyPhone);
            SqlParameter BedroomCount = new SqlParameter();
            BedroomCount.ParameterName = "@BedroomCount";
            BedroomCount.SqlDbType = SqlDbType.Int;
            BedroomCount.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(BedroomCount);
            SqlParameter BathroomCount = new SqlParameter();
            BathroomCount.ParameterName = "@BathroomCount";
            BathroomCount.SqlDbType = SqlDbType.Int;
            BathroomCount.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(BathroomCount);
          DataSet ds= objDB.GetDataSet(objCommand);
            if (ds.Tables[0].Rows.Count > 0)
            {
                home = new HomeDetails();
                home.HomeId = id;
                home.AddressNumber = objCommand.Parameters["@AddressNumber"].Value.ToString();
                home.AddressName = objCommand.Parameters["@AddressName"].Value.ToString();
                home.AddressCity = objCommand.Parameters["@AddressCity"].Value.ToString();
                home.AddressState = objCommand.Parameters["@AddressState"].Value.ToString();
                home.AddressZip = objCommand.Parameters["@AddressZip"].Value.ToString();
                home.PropertyType = objCommand.Parameters["@PropertyType"].Value.ToString();
                home.Heating = objCommand.Parameters["@Heating"].Value.ToString();
                home.Cooling = objCommand.Parameters["@Cooling"].Value.ToString();
                home.YearBuild = Convert.ToInt32(objCommand.Parameters["@YearBuild"].Value);
                home.Garage = objCommand.Parameters["@Garage"].Value.ToString();
                home.Utilities = objCommand.Parameters["@Utilities"].Value.ToString();
                home.AskingPrice = Convert.ToInt32(objCommand.Parameters["@AskingPrice"].Value);
              
                home.WorkAddressName = objCommand.Parameters["@WorkAddressName"].Value.ToString();
                home.WorkAddressNumber = objCommand.Parameters["@WorkAddressNumber"].Value.ToString();
                home.WorkEmail = objCommand.Parameters["@WorkEmail"].Value.ToString();
                home.RealEstateCompany = objCommand.Parameters["@RealEstateCompany"].Value.ToString();
                home.CompanyPhone = objCommand.Parameters["@CompanyPhone"].Value.ToString();
                home.BedroomCount = Convert.ToInt32(objCommand.Parameters["@BedroomCount"].Value);
                home.BathroomCount = Convert.ToInt32(objCommand.Parameters["@BathroomCount"].Value);
            }

            SqlCommand descriptionCommand = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetHomeDescription"
            };
            descriptionCommand.Parameters.AddWithValue("@HomeID", id);

            DataSet descriptionDataSet = objDB.GetDataSet(descriptionCommand);
            if (descriptionDataSet.Tables[0].Rows.Count > 0)
            {
                home.Description = descriptionDataSet.Tables[0].Rows[0]["Description"].ToString();
            }



            SqlCommand roomCommand = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetRoomsByHomeId"
            };
            roomCommand.Parameters.AddWithValue("@HomeID", id);

            DataSet roomDataSet = objDB.GetDataSet(roomCommand);
            foreach (DataRow row in roomDataSet.Tables[0].Rows)
            {
                home.Rooms.Add(new RoomDetails
                {
                    RoomType = row["RoomType"].ToString(),
                    Width = row["Width"] != DBNull.Value ? Convert.ToInt32(row["Width"]) : 0,
                    Length = row["Length"] != DBNull.Value ? Convert.ToInt32(row["Length"]) : 0
                });
            }


            SqlCommand amenitiesCommand = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetAmenitiesByHomeId"
            };
            amenitiesCommand.Parameters.AddWithValue("@HomeID", id);

            DataSet amenitiesDataSet = objDB.GetDataSet(amenitiesCommand);
            foreach (DataRow row in amenitiesDataSet.Tables[0].Rows)
            {
                home.Amenities.Add(row["AmenitiesName"].ToString());
            }


            SqlCommand imagesCommand = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetImagesByHomeId"
            };
            imagesCommand.Parameters.AddWithValue("@HomeID", id);
            DataSet imagesDataSet = objDB.GetDataSet(imagesCommand);
            foreach (DataRow row in imagesDataSet.Tables[0].Rows)
            {
                home.Images.Add(new HomeImageDetails
                {
                    ImageUrl = row["Imagie"].ToString()
                });
            }

            return home;
        }
       
        [HttpPost("AddHomeShowing")]
        public HomeShowingModel AddHomeShowing([FromBody] HomeShowingModel showing)
        {
            SqlCommand ShowingCommand = new SqlCommand();
            DBConnect dbConnection = new DBConnect();
            ShowingCommand.CommandText = "AddHomeShowing";
            ShowingCommand.CommandType = CommandType.StoredProcedure;
            if (showing == null)
            {
                HomeShowingModel showing1 = new HomeShowingModel();
                showing = showing1;
            }  
                ShowingCommand.Parameters.AddWithValue("@HomeId", showing.HomeId);
                ShowingCommand.Parameters.AddWithValue("@ShowingDate", showing.ShowingDate);
                ShowingCommand.Parameters.AddWithValue("@BuyerName", showing.BuyerName);
                ShowingCommand.Parameters.AddWithValue("@BuyerEmail", showing.BuyerEmail);
                ShowingCommand.Parameters.AddWithValue("@BuyerPhone", showing.BuyerPhone);
                dbConnection.DoUpdate(ShowingCommand);
            return showing;
        }

        [HttpPost("AddHomeOffer")]
        public HomeOfferModel AddHomeOffer([FromBody] HomeOfferModel offer)
        {
            SqlCommand OfferCommand = new SqlCommand();
            DBConnect dbConnection = new DBConnect();
            OfferCommand.CommandText = "AddHomeOffer";
            OfferCommand.CommandType = CommandType.StoredProcedure;
            if (offer == null)
            {
                HomeOfferModel offer1 = new HomeOfferModel();
                offer = offer1;
            }
            OfferCommand.Parameters.AddWithValue("@OfferName", offer.OfferName);
            OfferCommand.Parameters.AddWithValue("@OfferEmail", offer.OfferEmail);
            OfferCommand.Parameters.AddWithValue("@OfferPhone", offer.OfferPhone);
            OfferCommand.Parameters.AddWithValue("@OfferAmount", offer.OfferAmount);
            OfferCommand.Parameters.AddWithValue("@HomeId", offer.HomeId);
            OfferCommand.Parameters.AddWithValue("@SaleType", offer.SaleType);
            OfferCommand.Parameters.AddWithValue("@Contingencies", offer.Contingencies);
            OfferCommand.Parameters.AddWithValue("@NeedsToSellHome", offer.NeedsToSellHome);
            OfferCommand.Parameters.AddWithValue("@PreferredMoveInDate", offer.PreferredMoveInDate);
            dbConnection.DoUpdate(OfferCommand);
            return offer;
        }



        [HttpPost("AddBroker")]
        public bool AddBroker([FromBody]  BrokerProfile Profile)
        {
            //String Broker = "1";

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

                //password encryption process
                String plainTextPassword = Profile.UserPassword;
                String encryptedPassword;

                UTF8Encoding encoder = new UTF8Encoding();      // used to convert bytes to characters, and back
                Byte[] textBytes;                               // stores the plain text data as bytes

                textBytes = encoder.GetBytes(plainTextPassword);

                RijndaelManaged rmEncryption = new RijndaelManaged();
                MemoryStream myMemoryStream = new MemoryStream();
                CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

                myEncryptionStream.Write(textBytes, 0, textBytes.Length);
                myEncryptionStream.FlushFinalBlock();

                myMemoryStream.Position = 0;
                Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
                myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

                myEncryptionStream.Close();
                myMemoryStream.Close();

                encryptedPassword = Convert.ToBase64String(encryptedBytes);
                //end of encryption

                //parameter = new SqlParameter("@UserPassword", Profile.UserPassword);
                parameter = new SqlParameter("@UserPassword", encryptedPassword);
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


                DBConnect SecurityDB = new DBConnect();
                SqlCommand SecurityCommand = new SqlCommand();
                SecurityCommand.CommandType = CommandType.StoredProcedure;
                SecurityCommand.CommandText = "AddSecurityQuestions";

                SecurityCommand.Parameters.AddWithValue("@BrokerId", Profile.BrokerId);
                SecurityCommand.Parameters.AddWithValue("@Question1", Profile.SecurityQuestion1);
                SecurityCommand.Parameters.AddWithValue("@Answer1", Profile.SecurityAnswer1);
                SecurityCommand.Parameters.AddWithValue("@Question2", Profile.SecurityQuestion2);
                SecurityCommand.Parameters.AddWithValue("@Answer2", Profile.SecurityAnswer2);
                SecurityCommand.Parameters.AddWithValue("@Question3", Profile.SecurityQuestion3);
                SecurityCommand.Parameters.AddWithValue("@Answer3", Profile.SecurityAnswer3);
                SecurityDB.DoUpdate(SecurityCommand);
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

        [HttpPost("CreateNewHome")]
        public bool CreateNewHome([FromBody] AddHomeModel home)
        {
            SqlCommand sqlCommand = new SqlCommand();
            DBConnect dbConnection = new DBConnect();
            sqlCommand.CommandText = "InsertHome";
            sqlCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter returnParameter = new SqlParameter("returnValue", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            sqlCommand.Parameters.Add(returnParameter);

            //int profileid = int.Parse(Request.Cookies["ProfileID"]);

            sqlCommand.Parameters.AddWithValue("@Profile_ID", home.ProfileID);
            sqlCommand.Parameters.AddWithValue("@Address_Number", home.AddressNumber);
            sqlCommand.Parameters.AddWithValue("@Address_Name", home.AddressName);
            sqlCommand.Parameters.AddWithValue("@AddressCity", home.AddressCity);
            sqlCommand.Parameters.AddWithValue("@AddressState", home.AddressState);
            sqlCommand.Parameters.AddWithValue("@AddressZip", home.AddressZip);
            sqlCommand.Parameters.AddWithValue("@Property_Type", home.PropertyType);
            //size (null for now!!!)
            sqlCommand.Parameters.AddWithValue("@Heating", home.Heating);
            sqlCommand.Parameters.AddWithValue("@Cooling", home.Cooling);
            sqlCommand.Parameters.AddWithValue("@Year_Build", home.YearBuild);
            sqlCommand.Parameters.AddWithValue("@Garage", home.Garage);
            sqlCommand.Parameters.AddWithValue("@Utilities", home.Utilities);
            sqlCommand.Parameters.AddWithValue("@Description", home.Description);
            sqlCommand.Parameters.AddWithValue("@AskingPrice", home.AskingPrice);
            sqlCommand.Parameters.AddWithValue("@Status", home.Status);

            sqlCommand.Parameters.AddWithValue("@AmenitiesName", home.AmenitiesName);

            //Ammentities checkboxes and new ammentities table


            int rev = dbConnection.DoUpdate(sqlCommand);

            if (rev > 0)
                return true;
            else return false;
        }

    }
}
