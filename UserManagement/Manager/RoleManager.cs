using Base.API.Manager;
using Base.API.Repository;
using System.Data;
using UserManagement.Data;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Manager
{
    public class RoleManager : BaseManager<Role>, IRoleManager
    {
        public RoleManager(AppDbContext db) : base(new BaseRepository<Role>(db))
        {
        }
        public Role GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public Role GetByName(string name)
        {
            return GetFirstOrDefault(c => c.Name == name.Trim() && c.IsActive);
        }
    }
}
