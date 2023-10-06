using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class EntityTypeManager : BaseManager<EntityType>, IEntityTypeManager
    {
        public EntityTypeManager(DbContextClass db) : base(new BaseRepository<EntityType>(db))
        {

        }


        public EntityType GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
