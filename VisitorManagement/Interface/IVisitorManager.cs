using VisitorManagement.Models;
using System.Collections.Generic;
using Base.API.Interface.Manager;

namespace VisitorManagement.Interface
{
    interface IVisitorManager : IBaseManager<Visitor>
    {
        //ICollection<Visitor> GetAll();
        Visitor GetAllVisitor(string name);
        Visitor GetById(int id);
    }
    
}
