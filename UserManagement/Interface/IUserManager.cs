using Base.API.Interface.Manager;
using Base.API.Manager;
using System.Data;
using UserManagement.Models;

namespace UserManagement.Interface
{
    interface IUserManager:IBaseManager<UserInfo>
    {
        UserInfo GetUserInfoByUserName(string userName);
        UserInfo GetById(int id);
        UserInfo GetByEmailNo(string email);
        UserInfo GetByEmployeeId(int employeeId);
    }
}
