
using Base.API.Interface.Manager;
using Base.API.Manager;
using System.Data;
using AccessManagement.Models;

namespace AccessManagement.Interface
{
    interface IUserAccessManager : IBaseManager<UserAccess>
    {
        UserAccess GetById(long id);
        UserAccess GetByCardNo(string cardNo);

    }
}
