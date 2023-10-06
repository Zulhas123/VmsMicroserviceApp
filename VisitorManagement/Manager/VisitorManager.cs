using Base.API.Manager;
using Base.API.Repository;
using System.Collections.Generic;
using VisitorManagement.Data;
using VisitorManagement.Interface;
using VisitorManagement.Models;


namespace VisitorManagement.Manager
{
    public class VisitorManager : BaseManager<Visitor>, IVisitorManager
    {
        public VisitorManager(DbContextClass db) : base(new BaseRepository<Visitor>(db))
        {
        }

       

        public Visitor GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id==id);
        }

       public Visitor GetAllVisitor(string name)
        {
            return GetFirstOrDefault(c => c.Name == name);
        }

       
    }
  
}
