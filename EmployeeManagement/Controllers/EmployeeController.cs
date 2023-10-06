using Base.API.Controllers;
using EmployeeManagement.Data;
using EmployeeManagement.Manager;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace EmployeeManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class EmployeeController : BaseController
    {

        private readonly EmployeeManager _employeeManager;
        private readonly EmployeeDataManager _employeeDataManager;
        private readonly EmployeeConfigManager _employeeConfigManager;
        private readonly DepartmentManager _departmentManager;
        public EmployeeController(DbContextClass dbContext)
        {
            _employeeManager = new EmployeeManager(dbContext);
            _employeeDataManager = new EmployeeDataManager(dbContext);
            _employeeConfigManager = new EmployeeConfigManager(dbContext);
            _departmentManager = new DepartmentManager(dbContext);
        }

        // Employee Model 

        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            try
            {
                var Query = "Select e.id as EmployeeId,e.DepartmentId,e.EmployeeCode,e.Name as EmployeeName,d.Name As DepartmentName from Employee e INNER JOIN Department d ON e.DepartmentId=d.Id";
                var allEmployee = _employeeManager.ExecuteRawSql(Query);
                List<VmEmployee> employees = new List<VmEmployee>();
                employees = (from DataRow dtrow in allEmployee.Rows
                             select new VmEmployee()
                             {
                                 EmployeeId = dtrow["EmployeeId"].ToString(),
                                 DepartmentId = dtrow["DepartmentId"].ToString(),
                                 EmployeeCode = dtrow["EmployeeCode"].ToString(),
                                 EmployeeName = dtrow["EmployeeName"].ToString(),
                                 DepartmentName = dtrow["DepartmentName"].ToString(),
                             }).ToList();
                return Ok(employees);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                var existData = _employeeManager.GetById(id);
                if (existData == null)
                {
                    return NotFound();
                }

                return Ok(existData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult SaveEmployee(VmSaveEmployee data)
        {
            try
            {
                Employee emp = new Employee()
                {
                    EmployeeCode = data.EmployeeCode,
                    Name = data.EmployeeName,
                    DepartmentId= data.DepartmentId,
                    
                };
                AuditInsert(emp);
                //Save Employee
                if (_employeeManager.Add(emp))
                {
                    var employeeId = emp.Id;
                    List<EmployeeData> empDataList = new List<EmployeeData>();
                    foreach (var item in data.Datalist)
                    {//Save others data
                        EmployeeData obj = new EmployeeData()
                        {
                            EmployeeId = item.EmployeeId,
                            EmpConfigId= item.EmpConfigId,
                            Value = item.Value
                        };
                        empDataList.Add(obj);
                        AuditInsert(obj);
                    }
                    if (_employeeDataManager.Add(empDataList))
                    {
                        return Ok("Employee Save Succesfully");
                    }
                    else
                    {
                        return BadRequest("Employee Save Failed");
                    }
                }
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existData = _employeeManager.GetById(employee.Id);
                    if (existData == null)
                    {
                        //List<Employee> employees = new List<Employee>();
                        //Employee emp = new Employee();
                        //emp.Id = employee.Id;
                        //emp.EmpNameCode = employee.EmpNameCode;
                        //emp.Name = employee.Name;
                        //emp.DepartmentId= employee.DepartmentId;
                        //employees.Add(emp);

                        bool result = _employeeManager.Add(employee);
                        if (result)
                        {
                            return Ok("Add Employee Successfully");
                        }
                        else
                        {
                            return BadRequest("Employee Add Failed!!");
                        }
                    }
                    else
                    {
                        return Ok("This Employee already Exist!!");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

        [HttpPut]
        public IActionResult EditEmployee(Employee employee, int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existingData = _employeeManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    existingData.Name = employee.Name;
                    existingData.EmployeeCode = employee.EmployeeCode;
                    existingData.DepartmentId = employee.DepartmentId;
                    bool result = _employeeManager.Update(existingData);
                    if (result)
                    {
                        return Ok("Updated successfully");
                    }
                    else
                    {
                        return BadRequest("Update Failed");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

        [HttpDelete]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var existingData = _employeeManager.GetById(id);
                if (existingData == null)
                {
                    return NotFound();
                }
                bool result = _employeeManager.Delete(existingData);
                if (result)
                {
                    return Ok("Deleted successfully");
                }
                else
                {
                    return BadRequest("Delete failed!!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //*********************** Internal API From Employee Service To Card Service Call **************
        [HttpGet("{referenceId}")]
        public IActionResult GetEmployeeByRefId(string referenceId)
        {
            try
            {
                //string referenceId = "1,2,3";
                var query = "SELECT E.Id, E.Name as EmployeeName, E.EmployeeCode, D.Name as Department FROM Employee E INNER JOIN Department D ON E.DepartmentId = D.Id " +
                            "WHERE E.Id IN ({0}) AND E.IsActive = 1";

                var parameterizedQuery = string.Format(query, referenceId);
                var employeeData = _employeeManager.ExecuteRawSql(parameterizedQuery);

                List<VmEmployeeInfo> dataList = new List<VmEmployeeInfo>();
                dataList = (from DataRow dtRow in employeeData.Rows
                            select new VmEmployeeInfo()
                            {
                                EmployeeId = Convert.ToInt32(dtRow["Id"]),
                                EmployeeCode = dtRow["EmployeeCode"].ToString(),
                                Name = dtRow["EmployeeName"].ToString(),
                                Department = dtRow["Department"].ToString(),
                            }).ToList();
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //**********************Internal Api Call From Employee Service To Visitor Service ******************

        [HttpGet]
        public IActionResult GetEmployeebyTowhomId()
        {
            try
            {
                var employeeId = 2;
                var query = "SELECT E.Name as EmployeeName,E.EmployeeCode,D.Name as Department FROM Employee E INNER JOIN Department D ON E.DepartmentId=D.Id WHERE E.Id=" + employeeId;
                var EmployeeData = _employeeManager.ExecuteRawSql(query);

                List<VmEmployeeInfo> datalist = new List<VmEmployeeInfo>();
                datalist = (from DataRow dtRow in EmployeeData.Rows
                            select new VmEmployeeInfo()
                            {                                                                
                                Name = dtRow["EmployeeName"].ToString(),
                                EmployeeCode = dtRow["EmployeeCode"].ToString(),
                                Department = dtRow["Department"].ToString(),
                            }).ToList();

                return Ok(datalist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //EmployeeData ***************************************8

        [HttpGet]
        public IActionResult GetEmployeeData()
        {
            try
            {
                var query = "Select e.Name as EmplyeeName,c.PropertyName,c.DataType from EmployeeData AS d\r\nINNER JOIN Employee AS e ON d.EmployeeId=e.Id INNER JOIN EmployeeConfig AS c ON d.EmpConfigId=c.Id";
                var EmployeeData = _employeeDataManager.ExecuteRawSql(query);
                List<VmEmployeeData> Datalist = new List<VmEmployeeData>();
                Datalist = (from DataRow dtRow in EmployeeData.Rows
                            select new VmEmployeeData()
                            {
                                EmplyeeName = dtRow["EmplyeeName"].ToString(),
                                Value = dtRow["PropertyName"].ToString(),
                                DataType = dtRow["DataType"].ToString(),
                            }).ToList();
                return Ok(Datalist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult GetEmployeeDataById(int id)
        {
            try
            {
                var existData = _employeeDataManager.GetById(id);
                if (existData == null)
                {
                    return NotFound();
                }

                return Ok(existData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult AddEmployeeData(EmployeeData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    List<EmployeeData> DataList = new List<EmployeeData>();
                    //var getEmployee = _employeeManager.GetById(data.EmployeeId);
                    //var empConfig = _employeeConfig.GetById(data.EmpConfigId);
                    var allConfig = _employeeConfigManager.GetAll();

                    foreach (var item in allConfig)
                    {
                        EmployeeData employee = new EmployeeData();
                        employee.EmpConfigId = item.Id;
                        employee.EmployeeId = data.EmployeeId;
                        employee.Value = data.Value;
                        DataList.Add(employee);
                    }



                    bool reseult = _employeeDataManager.Add(DataList);
                    if (reseult)
                    {
                        return Ok("Added Successfully");
                    }
                    else
                    {
                        return BadRequest("Add failed");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

        [HttpPut]
        public IActionResult EditEmployeeData(EmployeeData data, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {

                    var existingData = _employeeDataManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound("Data Not Found");
                    }
                    existingData.EmployeeId = data.EmployeeId;
                    existingData.EmpConfigId = data.EmpConfigId;
                    existingData.Value = data.Value;
                    bool result = _employeeDataManager.Update(existingData);
                    if (result)
                    {
                        return Ok("Update successfully");
                    }
                    else
                    {
                        return BadRequest("Update failed");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"{ex.Message}");
                }
            }

        }

        [HttpDelete]
        public IActionResult DeleteEmployeeData(int id)
        {
            try
            {
                var existingData = _employeeDataManager.GetById(id);
                if (existingData == null)
                {
                    return NotFound();
                }
                bool result = _employeeDataManager.Delete(existingData);
                if (result)
                {
                    return Ok("Delete successfully");
                }
                else
                {
                    return BadRequest("Delete failde");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // Employee Config ****************************************


        [HttpGet]
        public IActionResult GetEmployeeConfig()
        {
            try
            {
                var sql = "Select * from EmployeeConfig;";
                var Data = _employeeConfigManager.ExecuteRawSql(sql);
                List<VmEmployeeConfig> Datalist = new List<VmEmployeeConfig>();
                Datalist = (from DataRow dtRow in Data.Rows
                            select new VmEmployeeConfig()
                            {
                                EmployeeConfigId = dtRow["id"].ToString(),
                                PropertyName = dtRow["PropertyName"].ToString(),
                                DataType = dtRow["DataType"].ToString(),
                                isEntity = dtRow["IsEntity"].ToString(),
                                isRequired = dtRow["IsRequired"].ToString()
                            }).ToList();
                return Ok(Datalist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetEmployeeConfigById(int id)
        {
            try
            {
                var existData = _employeeConfigManager.GetById(id);
                if (existData == null)
                {
                    return NotFound();
                }
                return Ok(existData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        public IActionResult AddEmployeeConfig(EmployeeConfig data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    bool result = _employeeConfigManager.Add(data);
                    if (result)
                    {
                        return Ok("Added Successfully");
                    }
                    else
                    {
                        return BadRequest("Add Failed!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut]
        public IActionResult EditEmployeeConfig(EmployeeConfig data, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existingData = _employeeConfigManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    existingData.PropertyName = data.PropertyName;
                    existingData.DataType = data.DataType;
                    existingData.IsEntity = data.IsEntity;
                    existingData.IsRequired = data.IsRequired;
                    bool result = _employeeConfigManager.Update(existingData);
                    if (result)
                    {
                        return Ok("Update successfully");
                    }
                    else
                    {
                        return BadRequest("Update Failed");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

        [HttpDelete]
        public IActionResult DeleteEmployeeConfig(int id)
        {
            try
            {
                var existingData = _employeeConfigManager.GetById(id);
                if (existingData == null)
                {
                    return NotFound();
                }
                bool result = _employeeConfigManager.Delete(existingData);
                if (result)
                {
                    return Ok("Deleted Employee Config successfully");
                }
                else
                {
                    return BadRequest("Delete Failed");
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
