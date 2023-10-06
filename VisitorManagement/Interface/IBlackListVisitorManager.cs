using Base.API.Interface.Manager;
using VisitorManagement.Models;

namespace VisitorManagement.Interface.Manager
{
    interface IBlackListVisitorManager:IBaseManager<BlackListVisitor>
    {
        ICollection<BlackListVisitor> GetAll();
        BlackListVisitor GetById(int id);
    }
}
