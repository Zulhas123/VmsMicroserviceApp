using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    interface IEntityTypeManager
    {
        EntityType GetById(int id);
    }
}
