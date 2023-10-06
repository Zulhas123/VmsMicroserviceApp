using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class DepartmentManager : BaseManager<Department>, IDepartmentManager
    {
        public DepartmentManager(DbContextClass db) : base(new BaseRepository<Department>(db))
        {

        }

        public ICollection<Department> GetAll()
        {
            return Get(c => true);
        }

        public Department GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
