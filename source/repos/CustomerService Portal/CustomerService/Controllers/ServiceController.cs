using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;

namespace CustomerService.Controllers
{
    public class ServiceController : ApiController
    {
        // GET: api/Service
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };

        }
     
        [HttpPost]
        [Route("PostCustomerData")]
        public IHttpActionResult PostService(ApiRequest apiRequest)
        {

            ApiResponse apiResponse = new ApiResponse();
            SAPbobsCOM.Company oCompany;

            try
            {             
                //int Rectcode;
                oCompany = new SAPbobsCOM.Company();
                oCompany.Server = "KANYITA";
                oCompany.CompanyDB = "SBODemoUS";
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                oCompany.DbUserName = "sa";
                oCompany.DbPassword = "kanyittah123";
                oCompany.UserName = "manager";
                oCompany.Password = "Pass@123";
                oCompany.UseTrusted = false;
                int ret = oCompany.Connect();

                if (ret != 0)
                {
                    apiResponse.ResCode = "086";
                    apiResponse.Description = oCompany.GetLastErrorDescription().ToString();
                }
                else
                {
                    try
                    {
                   
                        SAPbobsCOM.BusinessPartners BusinessPartners;
                        BusinessPartners = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                        BusinessPartners.CardCode = apiRequest.CardCode;
                        BusinessPartners.CardName = apiRequest.CardName;
                        BusinessPartners.CardForeignName = apiRequest.SirName;
                        BusinessPartners.Cellular = apiRequest.PhoneNumber;
                        BusinessPartners.EmailAddress = apiRequest.Email;
                        BusinessPartners.Notes = apiRequest.JoinCriteria;
                        BusinessPartners.Profession = apiRequest.Profession;
                        BusinessPartners.County = apiRequest.County;                       
                        BusinessPartners.UserFields.Fields.Item("Diocese").Value = apiRequest.Diocese;
                        BusinessPartners.UserFields.Fields.Item("Archdeaconry").Value = apiRequest.Archdeaconry;
                        BusinessPartners.UserFields.Fields.Item("Deanery").Value = apiRequest.Deanery;
                        BusinessPartners.UserFields.Fields.Item("Parish").Value = apiRequest.Parish;
                        BusinessPartners.UserFields.Fields.Item("Congregation").Value = apiRequest.Congregation;
                        int rst = BusinessPartners.Add();
                        if (rst != 0)
                        {
                            apiResponse.Description = oCompany.GetLastErrorDescription().ToString();
                        }

                        else
                        {                          
                            apiResponse.ResCode = "000";
                            apiResponse.Description = "Successffully Saved";                          
                        }

                    }
                    catch (Exception ex)
                    {
                        apiResponse.ResCode = "096";
                        apiResponse.Description = "Error: " + ex.ToString() + ex.InnerException.ToString();
                    }
                }
            }

            catch (Exception errMsg)
            {
                apiResponse.ResCode = "096";
                apiResponse.Description = "Error: " + errMsg.ToString() + errMsg.InnerException.ToString();
            }



            return Ok(apiResponse);
        }




        public class ApiResponse
        {
            public string Description { get; set; }
            public string ResCode { get; set; }
            
          

        }

        public class ApiRequest
        {
            public string CardCode { get; set; }
            public string  CardName { get; set; }
            public string SirName { get; set; }
           public string PhoneNumber { get; set; }
            public string Email { get; set; }
           public string JoinCriteria { get; set; }            
            public string Profession { get; set; }        
            public string County { get; set; }        
            public string Diocese { get; set; }
            public string Archdeaconry { get; set; }
            public string Deanery { get; set; }
            public  string Parish { get; set; }
            public string Congregation { get; set; }

        }

       
    }
}
