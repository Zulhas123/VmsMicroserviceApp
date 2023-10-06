using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    interface IDepartmentManager
    {
        ICollection<Department> GetAll();
        Department GetById(int id);
    }
}
