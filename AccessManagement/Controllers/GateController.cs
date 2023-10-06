using AccessManagement.Data;
using AccessManagement.Enums;
using AccessManagement.Interface;
using AccessManagement.Manager;
using AccessManagement.Models;
using AccessManagement.ViewModels;
using Base.API.Controllers;
using Base.API.SecurityExtension;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AccessManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    [MiddlewareFilter(typeof(MyBasicAuthenticationMiddlewarePipeline))]
    public class GateController : BaseController
    {
        private readonly IGateManager gateManager;
        public GateController(AppDbContext db)
        {
            gateManager = new GateManager(db);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                string sql = "Select Id,IpAddress,ControllerSn,DoorNo,Type FROM Gates where IsActive=1";
                var data = gateManager.ExecuteRawSql(sql);
                var list = (from DataRow dr in data.Rows
                            select new GatesVm()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                IpAddress = dr["IpAddress"].ToString(),
                                ControllerSn = dr["ControllerSn"].ToString(),
                                DoorNo = Convert.ToInt32(dr["DoorNo"]),
                                Type = dr["Type"].ToString()
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
                var data = gateManager.GetById(id);
                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        [HttpPost]
        public IActionResult Add([FromBody] Gates gates)
        {
            try
            {
                AuditInsert(gates);
                if (gateManager.Add(gates))
                {
                    return Ok(gates);
                }
                return BadRequest();
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }
        [HttpPut]
        public IActionResult Update([FromBody] GatesVm gate)
        {
            try
            {
                var gateData = gateManager.GetById(gate.Id);
              
                if (gateData != null)
                {
                    gateData.IpAddress = gate.IpAddress;
                    gateData.ControllerSn = gate.ControllerSn;
                    gateData.DoorNo = gate.DoorNo;
                    gateData.Type = (UserType)Enum.Parse(typeof(UserType), gate.Type);

                    AuditUpdate(gateData);
                    if (gateManager.Update(gateData))
                    {
                        return Ok(gateData);
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
                var role = gateManager.GetById(id);
                AuditDelete(role);
                if (gateManager.Update(role))
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
