using Base.API.Manager;
using Base.API.Repository;
using VisitorManagement.Data;
using VisitorManagement.Interface;
using VisitorManagement.Models;


namespace VisitorManagement.Manager
{
    public class VisitorApplicationManager : BaseManager<VisitorApplication>, IVisitorApplicationManager
    {
        public VisitorApplicationManager(DbContextClass db) : base(new BaseRepository<VisitorApplication>(db))
        {
        }

        public ICollection<VisitorApplication> GetAll()
        {
            return Get(c => true);
        }

        public VisitorApplication GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id==id);
        }

       
    }
}
