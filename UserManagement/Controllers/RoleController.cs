using Base.API.Controllers;
using Base.API.SecurityExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserManagement.Data;
using UserManagement.Interface;
using UserManagement.Manager;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
    public class RoleController : BaseController
    {
        private readonly IRoleManager roleManager;
        public RoleController(AppDbContext db)
        {
            roleManager = new RoleManager(db);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                string sql = "Select Id, Name, OrderNo  From Roles where IsActive=1";
                var data = roleManager.ExecuteRawSql(sql);
                var list = (from DataRow dr in data.Rows
                            select new RoleVm()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Name = dr["Name"].ToString(),
                                OrderNo = Convert.ToInt32(dr["OrderNo"])
                            }).ToList();
                return Ok(list);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                var data = roleManager.GetById(id);
                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        [HttpPost]
        public IActionResult Add([FromBody] Role role)
        {
            try
            {
                var checkName=roleManager.GetByName(role.Name);
                if(checkName != null)
                {
                    return BadRequest("Role already exist");
                }

                AuditInsert(role);
                if (roleManager.Add(role))
                {
                    return Ok(role);
                }
                return BadRequest();
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        [HttpPut]
        public IActionResult Update([FromBody] RoleVm role)
        {
            try
            {
                var getRole = roleManager.GetById(role.Id);



                var checkName = roleManager.GetByName(role.Name);
                if (checkName != null && getRole.Id!=checkName.Id)
                {
                    return BadRequest("Role already exist");
                }

               
                if (getRole != null)
                {
                    getRole.Name = role.Name;
                    getRole.OrderNo = role.OrderNo;
                    AuditUpdate(getRole);
                    if (roleManager.Update(getRole))
                    {
                        return Ok(getRole);
                    }
                }

                return BadRequest("Failed to update");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var role = roleManager.GetById(id);
                AuditDelete(role);
                if (roleManager.Update(role))
                {
                    return Ok("Successfully deleted");
                }
                return BadRequest("Deletation failed");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
