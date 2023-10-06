using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    interface IEmployeeDataManager
    {
        EmployeeData GetById(int id);

    }
}
