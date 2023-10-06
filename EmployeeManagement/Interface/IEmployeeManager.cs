using EmployeeManagement.Models;
using System.Collections.Generic;


namespace EmployeeManagement.Interface
{
    interface IEmployeeManager
    {
        ICollection<Employee> GetAll();
        Employee GetById(int id);
    }
}
