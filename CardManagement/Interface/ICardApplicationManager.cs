using Base.API.Interface.Manager;
using CardManagement.Models;


namespace CardManagement.Interface
{
    interface ICardApplicationManager:IBaseManager<CardApplication>
    {
        CardApplication GetById(int id);
        IEnumerable<CardApplication> NextApprovalLayer(int orderId);
    }
}
