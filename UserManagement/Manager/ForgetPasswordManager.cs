using Base.API.Manager;
using Base.API.Repository;
using System.Data;
using UserManagement.Data;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Manager
{
    public class ForgetPasswordManager : BaseManager<ForgetPassword>, IForgetPasswordManager
    {
        public ForgetPasswordManager(AppDbContext db) : base(new BaseRepository<ForgetPassword>(db))
        {
        }

        public ForgetPassword GetByEmail(string email)
        {
            return Get(c => c.Email == email).OrderByDescending(c => c.Id).FirstOrDefault();
        }
    }
}
