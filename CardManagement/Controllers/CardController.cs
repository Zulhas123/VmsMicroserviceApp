using Base.API.Controllers;
using Base.API.Models;
using Base.API.SecurityExtension;
using CardManagement.Data;
using CardManagement.Enums;
using CardManagement.Interface;
using CardManagement.Manager;
using CardManagement.Models;
using CardManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Org.BouncyCastle.Ocsp;
using System.Text;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Storage;

namespace CardManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class CardController : BaseController
    {
        private readonly ICardApplicationManager cardApplicationManager;
        private readonly CardHistoryManager cardHistoryManager;
        private readonly IConfiguration config;
        public string? ValidIssuer { get; }

        private readonly ApplicationDbContext _dbContext;

        public CardController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            cardApplicationManager = new CardApplicationManager(dbContext);
            cardHistoryManager = new CardHistoryManager(dbContext);
            config = configuration;
            string validIssuer = config.GetSection("JWT:ValidIssuer").Value;
            ValidIssuer = validIssuer;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            string sql = "Select * From CardApplication Where IsActive=1;";
            var cardAllData = cardApplicationManager.ExecuteRawSql(sql);
            try
            {
                List<CardApplicationVM> cardList = new List<CardApplicationVM>();
                cardList = (from DataRow dr in cardAllData.Rows
                            select new CardApplicationVM()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Type = dr["Type"].ToString(),
                                ReferenceId = dr["ReferenceId"].ToString(),
                                NextApprovalEmployee = dr["NextApprovalEmployee"].ToString(),
                                Order = dr["Order"].ToString(),
                                ApprovedEmployee = dr["ApprovedEmployee"].ToString(),
                                IsPrinted = dr["IsPrinted"].ToString(),
                                IsApprove = dr["IsApprove"].ToString(),
                                IsAccessGranted = dr["IsAccessGranted"].ToString(),
                                RejectedBy = dr["RejectedBy"].ToString(),
                            }).ToList();
                return Ok(cardList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public IActionResult GetById(int id)
        {
            var existData = cardApplicationManager.GetById(id);
            if (existData == null)
            {
                return NotFound("Data not found");
            }
            return Ok(existData);
        }

        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Add(CardType type, int refernceId)
        {
            try
            {
                CardApplication cardApplication = new CardApplication();
                AuditInsert(cardApplication);
                cardApplication.Type = type;
                cardApplication.ReferenceId = refernceId;
                var order = 1;
                cardApplication.Order = order;
                var url = ValidIssuer + "/employeeservice/ApprovalLayer/GetOrderAndTypeForCardApprovalLayer/" + Convert.ToInt32(type) + "/" + order;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

                string employeeId = "";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(json);
                    employeeId = jsonObject["employeeId"].ToString();
                }
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest("No approval person found");
                }
                cardApplication.NextApprovalEmployee = employeeId;
                var data = cardApplicationManager.Add(cardApplication);

                if (data)
                {
                    return OkResult(data);
                }
                else
                {
                    return BadRequestResult("Data not saved");
                }
            }
            catch (Exception ex)
            {
                return BadRequestResult(ex.Message);
            }
        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = "'%," + UserData.UserId + ",%'";
                string sql = "SELECT Id, Type, ReferenceId, NextApprovalEmployee FROM CardApplication WHERE NextApprovalEmployee LIKE " + val;
                var allreferenceId = cardApplicationManager.ExecuteRawSql(sql);

                List<CardResponse> list = new List<CardResponse>();
                foreach (DataRow dr in allreferenceId.Rows)
                {
                    CardResponse obj = new CardResponse();
                    var type = Convert.ToInt32(dr["Type"]);
                    var refId = dr["ReferenceId"].ToString();

                    if (type == (int)CardType.Visitor)
                    {
                        var url = ValidIssuer + "/visitorservice/visitor/GetCardRequest/" + refId;

                        var visitorDetails = GetAPICall(url).Result.FirstOrDefault();
                        if (visitorDetails != null)
                        {
                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.Name = visitorDetails.Name;
                            obj.MobileNo = visitorDetails.MobileNo;
                            obj.EmployeeCode = "";
                            obj.Department = "";
                            obj.Type = (CardType)Enum.ToObject(typeof(CardType), type);
                            list.Add(obj);
                        }
                    }
                    else
                    {
                        var url = ValidIssuer + "/employeeservice/Employee/GetEmployeeByRefId/" + refId;
                        var employeeDetails = GetAPICall(url).Result.FirstOrDefault();
                        if (employeeDetails != null)
                        {
                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.Name = employeeDetails.Name;
                            obj.MobileNo = "";
                            obj.EmployeeCode = employeeDetails.EmployeeCode;
                            obj.Department = employeeDetails.Department;
                            obj.Type = (CardType)Enum.ToObject(typeof(CardType), type);
                            list.Add(obj);
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequestResult(ex.Message);
            }
        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var getCard = cardApplicationManager.GetById(id);
                if (getCard == null)
                {
                    return NotFound("Data not found");
                }

                //******** NextApprovalEmployee LoginUserId Delete ********
                string userId = "," + UserData.UserId.ToString() + ",";
                String multiData = getCard.NextApprovalEmployee;

                string[] Data = multiData?.Split(userId);
                var first = Data[0].ToString();
                var second = Data[1].ToString();
                if (first == "" && second == "")
                {
                    int order = getCard.Order++;
                    CardType type = getCard.Type;
                    var url = ValidIssuer + "/employeeservice/ApprovalLayer/GetCardApprovalLayerById/" + type + "/" + order;
                    var referenceId = GetAPICall(url);
                    if (referenceId.Result == null)
                    {
                        getCard.IsApprove = true;
                        getCard.NextApprovalEmployee = "";
                        getCard.Order = 0;
                        getCard.ApprovedEmployee += userId.Remove(0, 1);
                    }
                    else
                    {
                        getCard.NextApprovalEmployee = referenceId.ToString();
                        getCard.Order = order;
                        getCard.ApprovedEmployee += userId.Remove(0, 1);
                    }
                }
                else
                {
                    if (first == "")
                    {
                        getCard.NextApprovalEmployee = "," + second;
                    }
                    else if (second == "")
                    {
                        getCard.NextApprovalEmployee = first + ",";
                    }
                    else
                    {
                        getCard.NextApprovalEmployee = first + ("," + second);
                    }

                    //****** ApprovedEmployee LoginUserId Add ******
                    if (string.IsNullOrEmpty(getCard.ApprovedEmployee))
                    {
                        getCard.ApprovedEmployee += userId;
                    }
                    else
                    {
                        userId = userId.Remove(0, 1);
                        getCard.ApprovedEmployee += userId;
                    }
                }
                var result = cardApplicationManager.Update(getCard);
                if (result)
                {
                    return Ok("Successfully Approved");
                }
                else
                {
                    return BadRequestResult("Approved Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequestResult(ex.Message);
            }
        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Reject(int Id)
        {
            try
            {
                var getCard = cardApplicationManager.GetById(Id);
                if (getCard == null)
                {
                    return NotFound("Data not found");
                }
                getCard.NextApprovalEmployee = "";
                getCard.ApprovedEmployee = "";
                getCard.Order = 0;
                getCard.IsActive = false;
                getCard.RejectedBy = UserData.UserId;
                AuditUpdate(getCard);
                var result = cardApplicationManager.Update(getCard);
                if (result)
                {
                    return Ok("Successfully Rejected");
                }
                else
                {
                    return BadRequestResult("Rejected Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequestResult(ex.Message);
            }
        }

        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> ApproveList()
        {
            try
            {
                string sql = "Select Id, Type, ReferenceId From CardApplication Where IsApprove=1 And IsPrinted=0;";
                var allreferenceId = cardApplicationManager.ExecuteRawSql(sql);
                List<CardResponse> list = new List<CardResponse>();
                foreach (DataRow dr in allreferenceId.Rows)
                {
                    CardResponse obj = new CardResponse();
                    var type = Convert.ToInt32(dr["Type"]);
                    var refId = dr["ReferenceId"].ToString();

                    if (type == (int)CardType.Visitor)
                    {
                        var url = ValidIssuer + "/visitorservice/visitor/GetCardRequest/" + refId;

                        var visitorDetails = GetAPICall(url).Result.FirstOrDefault();
                        if (visitorDetails != null)
                        {
                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.Name = visitorDetails.Name;
                            obj.MobileNo = visitorDetails.MobileNo;
                            obj.EmployeeCode = "";
                            obj.Department = "";
                            obj.Type = (CardType)Enum.ToObject(typeof(CardType), type);
                            list.Add(obj);
                        }
                    }
                    else
                    {
                        var url = ValidIssuer + "/employeeservice/Employee/GetEmployeeByRefId/" + refId;
                        var employeeDetails = GetAPICall(url).Result.FirstOrDefault();
                        if (employeeDetails != null)
                        {
                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.Name = employeeDetails.Name;
                            obj.MobileNo = "";
                            obj.EmployeeCode = employeeDetails.EmployeeCode;
                            obj.Department = employeeDetails.Department;
                            obj.Type = (CardType)Enum.ToObject(typeof(CardType), type);
                            list.Add(obj);
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Print(int Id)
        {
            try
            {
                var getCard = cardApplicationManager.GetById(Id);
                if (getCard == null)
                {
                    return NotFound("Data not found");
                }
                getCard.IsPrinted = true;
                var result = cardApplicationManager.Update(getCard);
                if (result)
                {
                    return Ok("Successfully Printed");
                }
                else
                {
                    return BadRequestResult("Printed Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> PrintList()
        {
            try
            {
                string sql = "Select Id, Type, ReferenceId From CardApplication Where IsPrinted=1 AND IsAccessGranted=0;";
                var allreferenceId = cardApplicationManager.ExecuteRawSql(sql);
                List<CardResponse> list = new List<CardResponse>();
                foreach (DataRow dr in allreferenceId.Rows)
                {
                    CardResponse obj = new CardResponse();
                    var type = Convert.ToInt32(dr["Type"]);
                    var refId = dr["ReferenceId"].ToString();

                    if (type == (int)CardType.Visitor)
                    {
                        var url = ValidIssuer + "/visitorservice/visitor/GetCardRequest/" + refId;

                        var visitorDetails = GetAPICall(url).Result.FirstOrDefault();
                        if (visitorDetails != null)
                        {
                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.Name = visitorDetails.Name;
                            obj.MobileNo = visitorDetails.MobileNo;
                            obj.EmployeeCode = "";
                            obj.Department = "";
                            obj.Type = (CardType)Enum.ToObject(typeof(CardType), type);
                            list.Add(obj);
                        }
                    }
                    else
                    {
                        var url = ValidIssuer + "/employeeservice/Employee/GetEmployeeByRefId/" + refId;
                        var employeeDetails = GetAPICall(url).Result.FirstOrDefault();
                        if (employeeDetails != null)
                        {
                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.Name = employeeDetails.Name;
                            obj.MobileNo = "";
                            obj.EmployeeCode = employeeDetails.EmployeeCode;
                            obj.Department = employeeDetails.Department;
                            obj.Type = (CardType)Enum.ToObject(typeof(CardType), type);
                            list.Add(obj);
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> GrantedAccess(SaveAccess saveAccess)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var getCard = cardApplicationManager.GetById(saveAccess.Id);
                if (getCard == null)
                {
                    return NotFound("Data not found");
                }
                getCard.IsAccessGranted = true;


                UserType type;
                if (getCard.Type == CardType.Contrctual)
                {
                    type = UserType.Contractual;
                }
                else if (getCard.Type == CardType.Permanent)
                {
                    type = UserType.Permanent;
                }
                else
                {
                    type = UserType.LongTimeVisitor;
                }

                var oldCardHistory = cardHistoryManager.AddCardHistory(getCard.ReferenceId, getCard.Type);
                oldCardHistory.IsActive = false;
                cardHistoryManager.Update(oldCardHistory);

                CardHistory history = new CardHistory()
                {
                    UHFCardNo = saveAccess.UhfValue,
                    HFCardNo = saveAccess.HfValue,
                    ReferenceId = getCard.ReferenceId,
                    Type = getCard.Type
                };
                AuditInsert(history);
                cardHistoryManager.Add(history);

                UserAccess accessReq = new UserAccess()
                {
                    EndAt = saveAccess.EndDate,
                    HFCardNo = saveAccess.HfValue,
                    StartedAt = saveAccess.StartDate,
                    UHFCardNo = saveAccess.UhfValue,
                    UserType = type

                };
                await transaction.CommitAsync();

                string jsonBody = JsonConvert.SerializeObject(accessReq);
                HttpContent requestBodyContent = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                var url = ValidIssuer + "/accessservice/UserAccess/Add";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");
                HttpResponseMessage response = await client.PostAsync(url, requestBodyContent);


                if (response.IsSuccessStatusCode)
                {
                    var result = cardApplicationManager.Update(getCard);



                    return Ok("Access Granted");
                }
                else
                {
                    return BadRequestResult("Failed To Grant Access");
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet]
        public async Task<List<CardResponse>> GetAPICall(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

                List<CardResponse> apiResponse = new List<CardResponse>();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        apiResponse = JsonConvert.DeserializeObject<List<CardResponse>>(jsonString);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return apiResponse;

            }
        }

    }
}
