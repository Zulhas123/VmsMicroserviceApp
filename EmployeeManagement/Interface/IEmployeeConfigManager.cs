using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    interface IEmployeeConfigManager
    {
        EmployeeConfig GetById(int id);
    }
}
