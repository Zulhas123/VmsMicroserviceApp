using Base.API.Manager;
using Base.API.Repository;
using System.Data;
using UserManagement.Data;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Manager
{
    public class UserManager : BaseManager<UserInfo>, IUserManager
    {
        public UserManager(AppDbContext db) : base(new BaseRepository<UserInfo>(db))
        {
        }

        public UserInfo GetByEmployeeId(int employeeId)
        {
            return GetFirstOrDefault(c => c.EmployeeId == employeeId && c.IsActive);
        }

        public UserInfo GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public UserInfo GetByEmailNo(string email)
        {
            return GetFirstOrDefault(c => c.Email == email && c.IsActive);
        }

        public UserInfo GetUserInfoByUserName(string userName)
        {
            return GetFirstOrDefault(c => c.UserName == userName);
        }
    }
}
