using AccessManagement.Data;
using AccessManagement.Interface;
using AccessManagement.Models;
using Base.API.Manager;
using Base.API.Repository;
using System.Data;

namespace AccessManagement.Manager
{
    public class UserAccessManager : BaseManager<UserAccess>, IUserAccessManager
    {
        public UserAccessManager(AppDbContext db) : base(new BaseRepository<UserAccess>(db))
        {
        }

        public UserAccess GetByCardNo(string cardNo)
        {
            return GetFirstOrDefault(c => (c.HFCardNo  == cardNo || c.UHFCardNo==cardNo ) && c.IsActive);
        }

        public UserAccess GetById(long id)
        {
            return GetFirstOrDefault(c=>c.Id == id);
        }
    }
}
