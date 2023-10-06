using Base.API.Manager;
using Base.API.Repository;
using System.Data;
using AccessManagement.Models;
using AccessManagement.Interface;
using AccessManagement.Data;

namespace AccessManagement.Manager
{
    public class GateManager : BaseManager<Gates>, IGateManager
	{
        public GateManager(AppDbContext db) : base(new BaseRepository<Gates>(db))
        {
        }

        public Gates GetByControllerAndDoorNo(string controllerSn, int doorNo)
        {
            return GetFirstOrDefault(c => c.IsActive && c.ControllerSn == controllerSn && c.DoorNo == doorNo);
        }

        public Gates GetById(int id)
        {
           return GetFirstOrDefault(c=>c.Id == id);
        }
    }
}
