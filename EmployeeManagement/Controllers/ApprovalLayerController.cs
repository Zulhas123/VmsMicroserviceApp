using Base.API.Models;
using Base.API.SecurityExtension;
using EmployeeManagement.Data;
using EmployeeManagement.Enums;
using EmployeeManagement.Interface;
using EmployeeManagement.Manager;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Http.Headers;

namespace EmployeeManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class ApprovalLayerController : Controller
    {
        private VisitorApprovalLayerManager _visitorApprovalLayerManager;
        private readonly CardApprovalLayerManager _cardApprovalLayerManager;
        private readonly EmployeeManager _employeeManager;
        //private readonly IUserManager userManager;
        public ApprovalLayerController(DbContextClass dbContext)
        {
            _visitorApprovalLayerManager = new VisitorApprovalLayerManager(dbContext);
            _cardApprovalLayerManager = new CardApprovalLayerManager(dbContext);
            _employeeManager = new EmployeeManager(dbContext);
        }
        [HttpGet]
        public IActionResult GetAllVisitorApprovalLayer()
        {
            try
            {
                string query = "Select v.Id as VisitorLayerId,v.DepartmemtId,v.EmployeeId,D.Name as DepartmentName,E.Name as EmployeeName,v.[Order],E.EmployeeCode from VisitorApprovalLayer As V \r\n " +
                               "INNER JOIN Department AS D On V.DepartmemtId=D.Id INNER JOIN Employee E ON V.EmployeeId=E.Id ";
                var allapproveVisitor = _visitorApprovalLayerManager.ExecuteRawSql(query);
              
                    List<VmDepartment> data = new List<VmDepartment>();
                    data = (from DataRow dtRow in allapproveVisitor.Rows
                            select new VmDepartment()
                            {
                                VisitorLayerId = dtRow["VisitorLayerId"].ToString(),
                                DepartmemtId = dtRow["DepartmemtId"].ToString(),
                                EmployeeId = dtRow["EmployeeId"].ToString(),
                                EmployeeCode = dtRow["EmployeeCode"].ToString(),
                                EmployeeName = dtRow["EmployeeName"].ToString(),
                                DepartmentName = dtRow["DepartmentName"].ToString(),
                                Order = dtRow["order"].ToString(),

                            }).ToList();

                    return Ok(data);
            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public IActionResult GetVisitorApprovalLayerById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                else
                {
                    var visitor = _visitorApprovalLayerManager.GetById(id);
                    if (visitor != null)
                    {
                        return Ok(visitor);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GetVisitorApprovalLayer By Employee Id
        [HttpGet]
        public IActionResult GetVisitorApprovalLayerByEmpId(int EmployeeId)
        {
            try
            {
                var getEmployee = _employeeManager.GetById(EmployeeId);
                var ApprovLayerEmp = _visitorApprovalLayerManager.GetbyEmpLidAsync(getEmployee.Id);
                if (ApprovLayerEmp != null)
                {
                    return Ok(ApprovLayerEmp);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult AddVisitorLayer(VisitorApprovalLayer layer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existdata = _visitorApprovalLayerManager.Equals(layer);
                    if (existdata == false)
                    {
                        bool result = _visitorApprovalLayerManager.Add(layer);
                        if (result)
                        {
                            return Ok("Add Visitor Approval layer Successfully");
                        }
                        else
                        {
                            return BadRequest("Add Failed");
                        }
                    }
                    else
                    {
                        return Ok("Allready exist!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut]
        public IActionResult EditVisitorLayer(VisitorApprovalLayer layer, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existingData = _visitorApprovalLayerManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existingData.EmployeeId = layer.EmployeeId;
                        existingData.DepartmemtId = layer.DepartmemtId;
                        existingData.Order = layer.Order;
                        bool result = _visitorApprovalLayerManager.Update(existingData);
                        if (result)
                        {
                            return Ok("Updated Successfully");
                        }
                        else
                        {
                            return BadRequest("Update Failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }
        [HttpDelete]
        public IActionResult DeleteVisitorLayer(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                else
                {
                    var existingData = _visitorApprovalLayerManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        bool result = _visitorApprovalLayerManager.Delete(existingData);
                        if (result)
                        {
                            return Ok("Delete Successfully");
                        }
                        else
                        {
                            return BadRequest("Delete failed!");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //********************************Internal API Employee Service To Visitor service Call **************************
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> GetEmployeeApprovalLayerByTowhom(bool IsSelfRegister, int TowhomId, int order)
        {
            try
            {
                var employeeIds = "";
                if (IsSelfRegister)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:5000");
                        HttpResponseMessage response = await client.GetAsync($"/userservice/User/GetByEmployeeId?employeeId={TowhomId}");
                        // Check System User Or Not.
                        if (response.IsSuccessStatusCode)
                        {
                            // Check if user has AccessforApprove permission in the approval layer
                            var isAccessforApprove = await _visitorApprovalLayerManager.GetbyEmpLidAsync(TowhomId);
                            // System user but no permission for approve, go to the approval layer
                            if (isAccessforApprove == null)
                            {
                                var list = await _visitorApprovalLayerManager.GetAllbyOrderAsync(order);
                                employeeIds = string.Join(",", list.Select(c => c.EmployeeId));
                                var model = new ApprovalLayerRes
                                {
                                    EmployeeId = "," + employeeIds + ",",
                                    IsFrontDesk = false
                                };
                                return Ok(model);
                            }
                            else
                            {
                                var model = new ApprovalLayerRes
                                {
                                    EmployeeId = TowhomId.ToString(),
                                    IsFrontDesk = false
                                };
                                return Ok(model);
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            var model = new ApprovalLayerRes
                            {
                                EmployeeId = "",
                                IsFrontDesk = true
                            };

                            //        return Ok(model);
                            //    }
                            //}
                            return BadRequest();
                        }
                        else
                        {
                            var employee = _employeeManager.GetById(UserData.UserId);
                            var list = await _visitorApprovalLayerManager.GetbyVisitorLeyar(employee.DepartmentId, order);
                            if (list != null)
                            {
                                employeeIds = string.Join(",", list.Select(c => c.EmployeeId));
                                var model = new ApprovalLayerRes
                                {
                                    EmployeeId = employeeIds,
                                    IsFrontDesk = false
                                };
                                return Ok(model);

                            }
                            else
                            {
                                var model = new ApprovalLayerRes
                                {
                                    EmployeeId = "",
                                    IsFrontDesk = true
                                };
                                return Ok(model);
                            }
                            // Other User Logic
                            // if not Self register.                   
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }



        // Start Region : Card Approval Layer*****************************

        [HttpGet]
        public IActionResult GetAllCardApproval()
        {
            try
            {
                string query = " Select c.Id as CardLayerId,c.EmployeeId,c.[Order] ,c.[Type],e.EmployeeCode,e.Name as EmployeeName from CardApprovalLayer As c INNER JOIN Employee e ON c.EmployeeId=e.Id;";
                var Cardlayer = _visitorApprovalLayerManager.ExecuteRawSql(query);
                if (ModelState.IsValid)
                {
                    List<VmDepartment> data = new List<VmDepartment>();
                    data = (from DataRow dtRow in Cardlayer.Rows
                            select new VmDepartment()
                            {
                                CardLayerId = dtRow["CardLayerId"].ToString(),
                                EmployeeId = dtRow["EmployeeId"].ToString(),
                                EmployeeCode = dtRow["EmployeeCode"].ToString(),
                                EmployeeName = dtRow["EmployeeName"].ToString(),
                                Order = dtRow["order"].ToString(),
                                Type = dtRow["Type"].ToString(),
                            }).ToList();

                    return Ok(data);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult GetCardApprovalLayerById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                else
                {
                    var visitor = _cardApprovalLayerManager.GetById(id);
                    if (visitor != null)
                    {
                        return Ok(visitor);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GetCardApprovalLayer By Employee Id
        [HttpGet]
        public IActionResult GetCardApprovalLayerByEmpId(int EmployeeId)
        {
            try
            {
                var getEmployee = _employeeManager.GetById(EmployeeId);
                var ApprovLayerByEmp = _cardApprovalLayerManager.GetbyEmpLid(getEmployee.Id);
                if (ApprovLayerByEmp != null)
                {
                    return Ok(ApprovLayerByEmp);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult AddCardApprovalLayer(CardApprovalLayer layer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existdata = _cardApprovalLayerManager.Equals(layer);
                    if (existdata == false)
                    {
                        bool result = _cardApprovalLayerManager.Add(layer);
                        if (result)
                        {
                            return Ok("Added Successfully");
                        }
                        else
                        {
                            return BadRequest("Added Failed!");
                        }
                    }
                    else
                    {
                        return Ok("Allready exist!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

        [HttpPut]
        public IActionResult EditCardApprovalLayer(CardApprovalLayer layer, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existingData = _cardApprovalLayerManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existingData.EmployeeId = layer.EmployeeId;
                        existingData.Order = layer.Order;
                        existingData.Type = layer.Type;
                        bool result = _cardApprovalLayerManager.Update(existingData);
                        if (result)
                        {
                            return Ok("Update successfully");
                        }
                        else
                        {
                            return BadRequest("Update Failed!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpDelete]
        public IActionResult DeleteCardApprovalLayer(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                else
                {
                    var existingData = _cardApprovalLayerManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        bool result = _cardApprovalLayerManager.Delete(existingData);
                        if (result)
                        {
                            return Ok("Delete Successfully");
                        }
                        else
                        {
                            return BadRequest("Delete Failed!!");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // *********************** Internal API Employee servicce To Card service Call *******************
        [HttpGet("{type}/{order}")]
        [MiddlewareFilter(typeof(MyBasicAuthenticationMiddlewarePipeline))]
        public IActionResult GetOrderAndTypeForCardApprovalLayer(int type, int order)
        {
            try
            {

                var list = _cardApprovalLayerManager.GetAllbyTypeAndOrder(order, type);
                if (list.Count == 0)
                {
                    return NotFound("Not Found Any Employee for this Type and Order!");
                }
                else
                {
                    var employeeIds = string.Join(",", list.Select(c => c.EmployeeId));
                    var emp = new CardApprLayerRes
                    {
                        employeeId = "," + employeeIds + ",",

                    };
                    return Ok(emp); ;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
