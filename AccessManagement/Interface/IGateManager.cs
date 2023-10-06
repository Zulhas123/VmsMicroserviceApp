using AccessManagement.Models;
using Base.API.Interface.Manager;
using Base.API.Manager;
using System.Data;

namespace AccessManagement.Interface
{
    interface IGateManager : IBaseManager<Gates>
    {
        Gates GetByControllerAndDoorNo(string controllerSn, int doorNo);
        Gates GetById(int id);
    }
}
