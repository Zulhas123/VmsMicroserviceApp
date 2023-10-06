using Base.API.Manager;
using Base.API.Repository;
using System.Data;
using UserManagement.Data;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Manager
{
    public class PermissionManager : BaseManager<Permission>, IPermissionManager
	{
        public PermissionManager(AppDbContext db) : base(new BaseRepository<Permission>(db))
        {
        }
    }
}
