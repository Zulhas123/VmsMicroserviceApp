using System.Collections.Generic;
using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class EmployeeManager : BaseManager<Employee>, IEmployeeManager
    {
        public EmployeeManager(DbContextClass db) : base(new BaseRepository<Employee>(db))
        {
        }

        public ICollection<Employee> GetAll()
        {
            return Get(c => true);
        }

        public Employee GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id==id);
        }

       
    }
}
