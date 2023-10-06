
using Base.API.Manager;
using Base.API.Repository;
using VisitorManagement.Data;
using VisitorManagement.Interface.Manager;
using VisitorManagement.Models;

namespace VisitorManagement.Manager
{
    public class VisitorBlackListManager:BaseManager<BlackListVisitor>, IBlackListVisitorManager
    {
        public VisitorBlackListManager(DbContextClass db) : base(new BaseRepository<BlackListVisitor>(db))
        {
        }

        public ICollection<BlackListVisitor> GetAll()
        {
            return Get(c => true);
        }

        public BlackListVisitor GetById(int id)
        {
           return GetFirstOrDefault(x=>x.Id==id);
        }
    }
}
