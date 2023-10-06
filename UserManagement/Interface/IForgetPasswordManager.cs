using Base.API.Interface.Manager;
using Base.API.Manager;
using System.Data;
using UserManagement.Models;

namespace UserManagement.Interface
{
    interface IForgetPasswordManager:IBaseManager<ForgetPassword>
    {
       public ForgetPassword GetByEmail(string email);
        
    }
}
