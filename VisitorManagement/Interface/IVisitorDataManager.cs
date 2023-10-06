using Base.API.Interface.Manager;
using VisitorManagement.Models;

namespace VisitorManagement.Interface.Manager
{
    interface IVisitorDataManager:IBaseManager<VisitorData>
    {
        ICollection<VisitorData> GetAll();
        VisitorData GetById(int id);
    }
}
