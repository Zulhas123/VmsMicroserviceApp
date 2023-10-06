using VisitorManagement.Models;

using System.Collections.Generic;
using Base.API.Interface.Manager;

namespace VisitorManagement.Interface
{
    interface IVisitorApplicationManager : IBaseManager<VisitorApplication>
    {
        ICollection<VisitorApplication> GetAll();
        VisitorApplication GetById(int id);
    }
}
