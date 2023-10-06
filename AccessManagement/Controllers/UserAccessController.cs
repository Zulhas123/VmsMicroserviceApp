using AccessManagement.Data;
using AccessManagement.Interface;
using AccessManagement.Manager;
using AccessManagement.Models;
using Base.API.Controllers;
using Base.API.SecurityExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
    public class UserAccessController : BaseController
    {
        private readonly IUserAccessManager _userAccessManager;
        private readonly IPunchHistoryManager _punchHistoryManager;
        private readonly IGateManager _gateManager;
        public UserAccessController(AppDbContext db)
        {
            _userAccessManager = new UserAccessManager(db);
            _punchHistoryManager = new PunchHistoryManager(db);
            _gateManager = new GateManager(db);
        }
        [HttpPost]
        public IActionResult Add([FromBody] UserAccess userAccess)
        {
            try
            {
                AuditInsert(userAccess);
                if (_userAccessManager.Add(userAccess))
                {
                    return OkResult(userAccess, "Saved successfully");
                }
                return BadRequestResult("Failed to save");
            }
            catch (Exception e)
            {

                return BadRequestResult(e.Message);
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] UserAccess userAccess)
        {
            try
            {
                var data = _userAccessManager.GetById(userAccess.Id);
                if (data == null)
                {
                    return BadRequestResult("No data found");
                }

                AuditUpdate(data);
                data.StartedAt = userAccess.StartedAt;
                data.EndAt = userAccess.EndAt;
                data.HFCardNo = userAccess.HFCardNo;
                data.UHFCardNo = userAccess.UHFCardNo;
                data.UserType = userAccess.UserType;

                if (_userAccessManager.Update(data))
                {
                    return OkResult(userAccess, "Updated successfully");
                }
                return BadRequestResult("Failed to update");
            }
            catch (Exception e)
            {

                return BadRequestResult(e.Message);
            }
        }
        [HttpDelete]
        public IActionResult Delete(long id)
        {
            try
            {
                var data = _userAccessManager.GetById(id);
                if (data == null)
                {
                    return BadRequestResult("No data found");
                }

                AuditUpdate(data);

                if (_userAccessManager.Update(data))
                {
                    return OkResult("Deleted successfully");
                }
                return BadRequestResult("Failed to delete");
            }
            catch (Exception e)
            {

                return BadRequestResult(e.Message);
            }
        }

        [HttpGet]
        public IActionResult CheckAccess(string cardNo, string controllerSn, int doorNo)
        {
            HasAccessVm res=new HasAccessVm();
            var currentTime = DateTime.Now;
            var gate = _gateManager.GetByControllerAndDoorNo(controllerSn, doorNo);
            if (gate != null)
            {
                var cardHolder = _userAccessManager.GetByCardNo(cardNo);

                if (cardHolder != null && gate.Type == cardHolder.UserType && cardHolder.EndAt >= currentTime)
                {

                    res = new HasAccessVm
                    {
                        HasAccess = true,
                        Ip = gate.IpAddress
                    };
                    PunchHistory obj = new PunchHistory()
                    {
                        Time = currentTime,
                        IpAddress = gate.IpAddress,
                        CardNo = cardNo,
                        ControllerSn = controllerSn,
                        DoorNo = doorNo
                    };
                    _punchHistoryManager.Add(obj);

                    return OkResult(res);

                }


            }
            return OkResult(res);

        }
    }
}
