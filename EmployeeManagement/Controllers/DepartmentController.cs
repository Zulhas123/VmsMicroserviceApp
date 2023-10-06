using Base.API.Controllers;
using EmployeeManagement.Data;
using EmployeeManagement.Manager;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace EmployeeManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class DepartmentController : BaseController
    {
        private readonly DepartmentManager _departmentManager;
        private readonly EmployeeManager _employeeManager;


        public DepartmentController(DbContextClass dbContext)
        {
            _departmentManager = new DepartmentManager(dbContext);
            _employeeManager= new EmployeeManager(dbContext);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var Query = "Select * from Department";
                var Deparments = _departmentManager.ExecuteRawSql(Query);
                List<Department> DepartmentList = new List<Department>();
                DepartmentList = (from DataRow dtRow in Deparments.Rows
                                  select new Department()
                                  {
                                      Id = Convert.ToInt32(dtRow["Id"]),
                                      Name = dtRow["Name"].ToString()
                                  }).ToList();
                return OkResult(DepartmentList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                var existData = _departmentManager.GetById(id);
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
        public IActionResult Add(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            else
            {
                try
                {
                    bool result = _departmentManager.Add(department);
                    if (result)
                    {
                        return OkResult("Department Save successfully");

                    }
                    else
                    {
                        return BadRequestResult("Failed to Save Department");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Department department, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");
            }
            else
            {
                try
                {
                    var existingData = _departmentManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existingData.Name = department.Name;
                        bool result = _departmentManager.Update(existingData);
                        if (result)
                        {
                            return Ok("Department Update Successfully");
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
        public IActionResult Delete(int id)
        {
            try
            {
                var existingData = _departmentManager.GetById(id);
                if (existingData == null)
                {
                    return NotFound();
                }
                bool result = _departmentManager.Delete(existingData);
                if (result)
                {
                    return Ok("Deleted Department successfully");
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
