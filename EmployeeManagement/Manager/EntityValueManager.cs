using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class EntityValueManager : BaseManager<EntityValue>, IEntityValueManager
    {
        public EntityValueManager(DbContextClass db) : base(new BaseRepository<EntityValue>(db))
        {

        }

        public ICollection<EntityValue> GetAll()
        {
            return Get(c => true);
        }

        public EntityValue GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
