using VisitorManagement.Models;
using System.Collections.Generic;
using Base.API.Interface.Manager;

namespace VisitorManagement.Interface.Manager
{
    interface IVisitorConfigurationManager : IBaseManager<VisitorConfiguration>
    {
        ICollection<VisitorConfiguration> GetAll();
        VisitorConfiguration GetById(int id);
    }
}
