using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class EmployeeDataManager : BaseManager<EmployeeData>, IEmployeeDataManager
    {
        public EmployeeDataManager(DbContextClass db) : base(new BaseRepository<EmployeeData>(db))
        {

        }

        public EmployeeData GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
