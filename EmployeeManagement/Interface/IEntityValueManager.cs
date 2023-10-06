using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    interface IEntityValueManager
    {
        ICollection<EntityValue> GetAll();
        EntityValue GetById(int id);
    }
}
