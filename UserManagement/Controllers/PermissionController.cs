using Base.API.Controllers;
using Base.API.SecurityExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
   
    public class PermissionController : BaseController
    {
        private readonly IPermissionManager permissionManager;
        public PermissionController(AppDbContext db)
        {
            permissionManager = new PermissionManager(db);
        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyBasicAuthenticationMiddlewarePipeline))]
        public IActionResult GetSubMenuByRoleId(int roleId)
        {
            try
            {
                var list = GetPermissionVmList(roleId);
                return Ok(list);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        internal List<PermissionVm> GetPermissionVmList(int roleId)
        {
            string sql = "Select p.Id, s.Name,s.Host, s.Path,s.RouteValue,s.MenuName,s.LabelName,s.ModuleName FROM Permissions as p join Submenus as s on p.SubmenuId=s.Id where p.RoleId=" + roleId;
            var data = permissionManager.ExecuteRawSql(sql);
            var list = (from DataRow dr in data.Rows
                        select new PermissionVm()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Host = dr["Host"].ToString(),
                            Path = dr["Path"].ToString(),
                            RouteValue = dr["RouteValue"].ToString(),
                            MenuName = dr["MenuName"].ToString(),
                            LabelName = dr["MenuName"].ToString(),
                            ModuleName = dr["ModuleName"].ToString(),


                        }).ToList();
            return list;
        }
        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add([FromBody] List<Permission> permission)
        {
            try
            {
                Delete(permission.FirstOrDefault()?.RoleId ?? 0);
                
                foreach (var item in permission)
                {
                    AuditInsert(item);
                }

                if (permissionManager.Add(permission))
                {
                    return Ok(permission);
                }
               

                return BadRequest("Not saved");
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        private int Delete(int roleId)
        {
            var sql = "delete from Permissions where RoleId=" + roleId;
            int res = permissionManager.ExecuteNonQuerySql(sql);
            return res;
        }


    }
}
