using Base.API.Manager;
using Base.API.Repository;
using VisitorManagement.Data;
using VisitorManagement.Interface.Manager;
using VisitorManagement.Models;


namespace VisitorManagement.Manager
{
    public class VisitorDataManager:BaseManager<VisitorData>,IVisitorDataManager
    {
        public VisitorDataManager(DbContextClass db) : base(new BaseRepository<VisitorData>(db)) 
          { 
          }

        public ICollection<VisitorData> GetAll()
        {
            return Get(c => true);
        }

        public VisitorData GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }
    }
}
