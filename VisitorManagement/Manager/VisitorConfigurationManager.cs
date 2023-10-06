using Base.API.Manager;
using Base.API.Repository;
using System.Collections.Generic;
using VisitorManagement.Data;
using VisitorManagement.Interface.Manager;
using VisitorManagement.Models;



namespace VisitorManagement.Manager
{
    public class VisitorConfigurationManager : BaseManager<VisitorConfiguration>, IVisitorConfigurationManager
    {
        public VisitorConfigurationManager(DbContextClass db) : base(new BaseRepository<VisitorConfiguration>(db))
        {
        }

        public ICollection<VisitorConfiguration> GetAll()
        {
            return Get(c => true);
        }

        public VisitorConfiguration GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id==id);
        }

       
    }
}
