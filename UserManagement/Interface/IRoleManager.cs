using Base.API.Interface.Manager;
using Base.API.Manager;
using System.Data;
using UserManagement.Models;

namespace UserManagement.Interface
{
    interface IRoleManager:IBaseManager<Role>
    {
        Role GetById(int id);
        Role GetByName(string name);
    }
}
