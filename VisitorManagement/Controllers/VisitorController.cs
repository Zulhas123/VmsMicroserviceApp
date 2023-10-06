using Base.API.Controllers;
using Base.API.Models;
using Base.API.SecurityExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using VisitorManagement.Data;
using VisitorManagement.Enums;
using VisitorManagement.Interface;
using VisitorManagement.Interface.Manager;
using VisitorManagement.Manager;
using VisitorManagement.Models;
using VisitorManagement.ViewModel;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace VisitorManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class VisitorController : BaseController
    {

        private readonly IVisitorManager _visitorManager;
        private readonly IVisitorConfigurationManager _visitorConfigurationManager;
        private readonly IBlackListVisitorManager _blackListVisitorManager;
        private readonly IVisitorApplicationManager _visitorApplicationManager;
        private readonly IVisitorDataManager _visitorDataManager;

        private readonly IConfiguration config;
        public string? ValidIssuer { get; }
        private readonly DbContextClass _dbContext;


        public VisitorController(DbContextClass db, IConfiguration configuration)
        {
            _visitorManager = new VisitorManager(db);
            _visitorConfigurationManager = new VisitorConfigurationManager(db);
            _blackListVisitorManager = new VisitorBlackListManager(db);
            _visitorApplicationManager = new VisitorApplicationManager(db);
            _visitorDataManager = new VisitorDataManager(db);

            config = configuration;
            string validIssuer = config.GetSection("JWT:ValidIssuer").Value;
            ValidIssuer = validIssuer;
            _dbContext = db;
        }
        [HttpGet]
        public IActionResult GetAllVisitor()
        {
            try
            {
                //var visitors=_visitorManager.GetAll();
                // var visitors = _db.Visitor.FromSql($"Select * from Visitor");

                //return Ok(visitors);
                string sql = "Select * from Visitor where IsActive=1";
                var visitor = _visitorManager.ExecuteRawSql(sql);
                List<VisitorVM> VisitorList = new List<VisitorVM>();
                VisitorList = (from DataRow dr in visitor.Rows
                               select new VisitorVM()
                               {
                                   Id = Convert.ToInt32(dr["Id"]),
                                   Name = dr["Name"].ToString(),
                                   Photo = dr["Photo"].ToString(),
                                   MobileNo = dr["MobileNo"].ToString(),
                                   Signature = dr["Signature"].ToString(),

                               }).ToList();
                //foreach (DataRow dtRow in users.Rows)
                //{
                //    var roleId = dtRow[""];
                //}
                return Ok(VisitorList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        [HttpGet("{id}")]
        public IActionResult GetSingleVisitor(int id)
        {
            try
            {
                //var visitor = _db.Visitor.FromSql($"Select * from Visitor where Id={id}");
                var visitor = _visitorManager.GetById(id);
                if (visitor == null)
                {
                    return BadRequest("This id's visitor not found.");
                }

                return Ok(visitor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //[HttpPost]
        //public IActionResult CreateVisitor(Visitor visitors)
        //{
        //    try
        //    {

        //        //string sql = "Select * from VisitorConfiguration";
        //        //var visitor = _visitorConfigurationManager.ExecuteRawSql(sql);
        //        //List<Visitor> VisitorList = new List<Visitor>();
        //        //VisitorList = (from DataRow dr in visitor.Rows
        //        //               select new Visitor()
        //        //               {
        //        //                   Id = Convert.ToInt32(dr["Id"]),
        //        //                   Name = dr["Name"].ToString(),
        //        //                   Photo = dr["Photo"].ToString(),
        //        //                   MobileNo = dr["MobileNo"].ToString(),
        //        //                   Signature = dr["Signature"].ToString(),

        //        //               }).ToList();


        //        AuditInsert(visitors);

        //        bool isSaved = _visitorManager.Add(visitors);


        //        if (isSaved)
        //        {
        //            return Ok("Visitor added successfully.");
        //        }
        //        return BadRequest("Visitor save failed.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost]
        public async Task<IActionResult> CreateVisitor(SaveVisitorVm data)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                Visitor visitor = new Visitor()
                {
                    MobileNo = data.MobileNo,
                    Name = data.Name
                };
                AuditInsert(visitor);
                //Save visitor 
                if (_visitorManager.Add(visitor))
                {
                    var visitorId = visitor.Id;

                    List<VisitorData> vDataList = new List<VisitorData>();
                    foreach (var item in data.VisitorData)
                    {//Save others data
                        VisitorData obj = new VisitorData()
                        {
                            VisitorConfiugurtationId = item.VisitorConfiugurtationId,
                            Value = item.Value,
                            VisitorId = visitorId

                        };
                        vDataList.Add(obj);
                    }
                    _visitorDataManager.Add(vDataList);

                    //Save application
                    VisitorApplication visitorApplication = new VisitorApplication()
                    {
                        ToWhom = data.ToWhom,
                        Purpose = data.Purpose,
                        VisitDate = data.VisitDate,
                        VisitorId = visitorId,
                        Status = Status.Pending,
                                               
                    };
                    visitorApplication.Order = 1;
                    var url = ValidIssuer + "/employeeservice/ApprovalLayer/GetEmployeeApprovalLayerByTowhom/" + data.IsSelfRegister + "/" + data.ToWhom + "/" + visitorApplication.Order;

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<ApprovalLayerRes>(json);

                        if (res != null)
                        {

                            if (res.IsFrontDesk)
                            {
                                visitorApplication.IsFrontDesk = true;
                                visitorApplication.Order = 0;
                                visitorApplication.NextApprovalEmpId = "";
                            }
                            else
                            {
                                visitorApplication.NextApprovalEmpId = res.EmployeeId;
                                visitorApplication.Order = 1;
                            }

                            _visitorApplicationManager.Add(visitorApplication);
                            await transaction.CommitAsync();
                            return Ok("Visitor information save successfully.");
                        }
                    }
                }
                return BadRequest("Visitor information does not save.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListByUserId()
        {
            try
            {
                UserData.UserId = 2;
                var val = "'%," + UserData.UserId + ",%'";
                string sql = "SELECT va.Id,va.ToWhom, va.NextApprovalEmpId,v.Name,v.MobileNo,va.VisitDate,va.Status,va.Purpose FROM VisitorApplication va INNER JOIN Visitor v on va.Id=v.Id WHERE NextApprovalEmpId LIKE" + val;
                var allreferenceId = _visitorApplicationManager.ExecuteRawSql(sql);

                List<VisitorApproveVm> list = new List<VisitorApproveVm>();
                
                foreach (DataRow dr in allreferenceId.Rows)
                {
                    VisitorApproveVm obj=new VisitorApproveVm();

                    var refId = dr["ToWhom"].ToString();


                    var url = ValidIssuer + "/employeeservice/Employee/GetEmployeeByRefId/" + refId;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

                     
                        HttpResponseMessage response = await client.GetAsync(url);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonString = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonConvert.DeserializeObject<List<EmployeeDetailsVm>>(jsonString).FirstOrDefault();

                            obj.VisitorName = dr["Name"].ToString();
                            obj.VisitorMobileNo = dr["MobileNo"].ToString();
                            obj.VisitDate = dr["VisitDate"].ToString();
                            obj.Status = dr["Status"].ToString();
                            obj.Purpose = dr["Purpose"].ToString();
                            obj.ToWhom = apiResponse?.Name;
                            obj.Department = apiResponse?.Department;
                            obj.EmployeeCode = apiResponse?.EmployeeCode;
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
        //Approve Api
        [HttpGet("{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                UserData.UserId = 2;
                var visitor = _visitorApplicationManager.GetById(id);
                if (visitor == null)
                {
                    return NotFound("Data not found");
                }

                //******** NextApprovalEmployee LoginUserId Delete ********
                string userId = "," + UserData.UserId.ToString() + ",";
                String multiData = visitor.NextApprovalEmpId;
                string[] Data = multiData.Split(userId);
                var first = Data[0].ToString();
                var second = Data[1].ToString();
                if (first == "" && second == "")
                {
                    int order = visitor.Order++;

                    var url = ValidIssuer + "/employeeservice/ApprovalLayer/GetEmployeeApprovalLayerByTowhom/" + order;
                    var referenceId = GetAPICall(url);
                    if (referenceId == null)
                    {
                        visitor.IsApprove = true;
                        visitor.NextApprovalEmpId = "";
                        visitor.ApprovedEmployeeId += userId;
                    }
                    else
                    {
                        visitor.NextApprovalEmpId = referenceId.ToString();
                        visitor.ApprovedEmployeeId += userId;
                    }
                }
                else
                {
                    if (first == "")
                    {
                        visitor.NextApprovalEmpId = "," + second;
                    }
                    else if (second == "")
                    {
                        visitor.NextApprovalEmpId = first + ",";
                    }
                    else
                    {
                        visitor.NextApprovalEmpId = first + ("," + second);
                    }

                    //****** ApprovedEmployee LoginUserId Add ******
                    if (visitor.ApprovedEmployeeId == "")
                    {
                        visitor.ApprovedEmployeeId += userId;
                    }
                    else
                    {
                        userId = userId.Remove(0, 1);
                        visitor.ApprovedEmployeeId += userId;
                    }
                    var result = _visitorApplicationManager.Update(visitor);
                    if (result)
                    {
                        return Ok("Successfully Approved");
                    }
                    else
                    {
                        return BadRequestResult("Approved Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequestResult(ex.Message);
            }
            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult RejectVisitor(int id)
        {
            try
            {
                var visitors = _visitorApplicationManager.GetById(id);
                if (visitors == null)
                {
                    return BadRequest("This id's visitor not found.");
                }
                AuditUpdate(visitors);
                visitors.NextApprovalEmpId = "";
                visitors.Order = 0;
                //visitors.Id = visitorApplication.Id;
                visitors.RejectedBy = UserData.UserId;

                bool isUpdate = _visitorApplicationManager.Update(visitors);
                if (isUpdate)
                {
                    return Ok("Visitor Rejected Successfully.");
                }
                return BadRequest("Visitor reject fail.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //ListUserInformation


        //[HttpPut("{id}")]
        //public IActionResult UpdateVisitor(Visitor visitor, int id)
        //{
        //    try
        //    {
        //        var visitors = _visitorManager.GetById(id);
        //        if (visitors == null)
        //        {
        //            return BadRequest("This id's visitor not found.");
        //        }
        //        visitors.UpdatedBy = GetUserId();
        //        visitors.UpdatedAt = DateTime.Now;
        //        visitors.Name = visitor.Name;
        //        visitors.MobileNo = visitor.MobileNo;
        //        visitors.Photo = visitor.Photo;
        //        visitors.Signature = visitor.Signature;

        //        bool isUpdate = _visitorManager.Update(visitors);
        //        if (isUpdate)
        //        {
        //            return Ok("Visitor Update Successfully.");
        //        }
        //        return BadRequest("Visitor update fail.");

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteVisitor(int id)
        //{
        //    try
        //    {
        //        var visitor = _visitorManager.GetById(id);
        //        if (visitor == null)
        //        {
        //            return Ok("This id's visitor not found.");
        //        }

        //        bool isDelete = _visitorManager.Delete(visitor);
        //        if (isDelete)
        //        {
        //            return Ok("Visitor has been deleted");
        //        }
        //        return Ok("Visitor delete failed");

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
      /// <summary>
      /// Configuration
      /// </summary>
      /// <returns></returns>
        
        [HttpGet]
        public IActionResult GetAllProperty()
        {
            try
            {
                string sql = "Select * from VisitorConfiguration where IsActive=1";
                var property = _visitorConfigurationManager.ExecuteRawSql(sql);
                List<VisitorConfigureVM> VisitorList = new List<VisitorConfigureVM>();
                VisitorList = (from DataRow dr in property.Rows
                               select new VisitorConfigureVM()
                               {
                                   Id = Convert.ToInt32(dr["Id"]),
                                   PropertyName = dr["PropertyName"].ToString(),
                                   Type = dr["DataType"].ToString(),
                                   IsRequired = dr["IsRequired"].ToString(),

                               }).ToList();
                return Ok(VisitorList);
                //var visitors=_visitorConfigurationManager.GetAll();                
                //return Ok(visitors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //[HttpGet("{id}")]
        //public IActionResult GetSingleProperty(int id)
        //{
        //    try
        //    {
        //        var visitor = _visitorConfigurationManager.GetById(id);
        //        if (visitor == null)
        //        {
        //            return BadRequest("This id's property not found.");
        //        }

        //        return Ok(visitor);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
        [HttpPost]
        public IActionResult CreateProperty(VisitorConfiguration visitorConfiguration)
        {
            try
            {
                AuditInsert(visitorConfiguration);
                bool isSaved = _visitorConfigurationManager.Add(visitorConfiguration);
                if (isSaved)
                {
                    return Ok("Property added successfully.");
                }
                return BadRequest("Property save failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public IActionResult UpdateProperty(VisitorConfiguration visitorConfiguration)
        {
            try
            {
                    var existingData = _visitorConfigurationManager.GetById(visitorConfiguration.Id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    existingData.PropertyName = visitorConfiguration.PropertyName;
                    existingData.DataType= visitorConfiguration.DataType;
                    existingData.IsRequired= visitorConfiguration.IsRequired;                  
                    bool isUpdate = _visitorConfigurationManager.Update(existingData);
                    if (isUpdate)
                    {
                        return Ok("Property updated successfully.");
                    }
                    else
                    {
                        return BadRequest($"{existingData.PropertyName} not found");
                    }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult DeleteProperty(int id)
        {
            var data=_visitorConfigurationManager.GetById(id);
            if(data==null)
            {
                return NotFound();
            }
            AuditDelete(data);
            bool isDelete=_visitorConfigurationManager.Update(data);
            if(isDelete)
            {
                return Ok("Property deleted successfully.");
            }
            else
            {
                return BadRequest($"{data.PropertyName} not found");
            }
        }
   
        /// <summary>
        /// Black list
        /// </summary>
        /// <returns></returns>
        
        
        [HttpGet]

        public IActionResult GetBlackList()
        {
            try
            {
                string sql = "select * from BlackListVisitor";
                var blackList = _blackListVisitorManager.ExecuteRawSql(sql);
                List<BlackListVM> result = new List<BlackListVM>();
                result = (from DataRow dtRow in blackList.Rows
                          select new BlackListVM()
                          {
                              Id = Convert.ToInt32(dtRow["Id"]),
                              VisitorId = dtRow["VisitorId"].ToString(),
                              MobileNo = dtRow["MobileNo"].ToString(),

                          }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]

        public IActionResult AddBlackList(BlackListVisitor blackListVisitor)
        {
            try
            {
                AuditInsert(blackListVisitor);
                bool isSave = _blackListVisitorManager.Add(blackListVisitor);
                if (isSave)
                {
                    return Ok("Visitor successfully added blacklist");
                }
                else
                {
                    return BadRequest("Visitor does not added blacklist");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveBlackList(int id)
        {
            try
            {
                var visitor = _blackListVisitorManager.GetById(id);
                if (visitor == null)
                {
                    return Ok("This id's visitor not found.");
                }

                bool isRemove = _blackListVisitorManager.Delete(visitor);
                if (isRemove)
                {
                    return Ok("Visitor has been deleted from blacklist");
                }
                return Ok("Visitor delete failed");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

       

      
        //Receive ApiCall from CardService
        [HttpGet("{referenceId}")]
        public async Task<IActionResult> GetCardRequest(string referenceId)
        {
            try
            {

                var query = "SELECT v.Name,v.MobileNo FROM Visitor AS v WHERE v.Id IN ({0});";
                var parameterizedQuery = string.Format(query, referenceId);
                var vData = _visitorManager.ExecuteRawSql(parameterizedQuery);
                List<VisitorVM> visitorvm = new List<VisitorVM>();
                visitorvm = (from DataRow dtRow in vData.Rows
                             select new VisitorVM()
                             {
                                 Name = dtRow["Name"].ToString(),
                                 MobileNo = dtRow["MobileNo"].ToString()
                             }).ToList();
                return Ok(visitorvm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //request api for next approve person
        //[HttpPost("{refernceId}")]
        //public async Task<IActionResult> CallNextApprove(int refernceId)
        //{
        //    try
        //    {
        //        VisitorApplication vApplication = new VisitorApplication();
        //        AuditInsert(vApplication);

        //        vApplication.ToWhom = refernceId;
        //        var order = 1;
        //        vApplication.Order = order;
        //        var url = ValidIssuer + "/employeeservice/ApprovalLayer/GetEmployeeApprovalLayerByTowhom/" + order;

        //        string employeeDetails = await AddAPICall(url);
        //        if (string.IsNullOrEmpty(employeeDetails))
        //        {
        //            return BadRequest("No approval person found");
        //        }
        //        vApplication.NextApprovalEmpId = employeeDetails;
        //        var data = _visitorApplicationManager.Add(vApplication);

        //        if (data)
        //        {
        //            return OkResult(data);
        //        }
        //        else
        //        {
        //            return BadRequestResult("Data not saved");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequestResult(ex.Message);
        //    }
        //}
        [HttpGet]
        public async Task<string> AddAPICall(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(json);

                        // Get the value of "employeeId"
                        string employeeId = jsonObject["employeeId"].ToString();
                        return employeeId;
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        //collect EmployeeDetails from employee service

        [HttpGet]
        public async Task<List<ApprovalLayerRes>> GetAPICall(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

                List<ApprovalLayerRes> apiResponse = new List<ApprovalLayerRes>();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        apiResponse = JsonConvert.DeserializeObject<List<ApprovalLayerRes>>(jsonString);
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
        //Approve List who are approve already
        [HttpPost]
        public async Task<IActionResult> ApproveList()
        {
            try
            {
                string sql = "Select * From VisitorApplication Where IsApprove=1;";
                var cardAllData = _visitorApplicationManager.ExecuteRawSql(sql);
                string refId = "";

                foreach (DataRow dr in cardAllData.Rows)
                {
                    refId += dr["ReferenceId"].ToString() + ",";
                }
                refId = refId.Substring(0, refId.Length - 1);

                var url = ValidIssuer + "/employeeservice/Employee/GetEmployeeByRefId/" + refId;
                var employeeDetails = await GetAPICall(url);
                return Ok(employeeDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}
