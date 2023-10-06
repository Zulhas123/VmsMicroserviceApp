using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;
namespace EmployeeManagement.Manager
{
    public class EmployeeConfigManager: BaseManager<EmployeeConfig>, IEmployeeConfigManager
    {
        public EmployeeConfigManager(DbContextClass db) : base(new BaseRepository<EmployeeConfig>(db))
        {
        }

        

        public EmployeeConfig GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
