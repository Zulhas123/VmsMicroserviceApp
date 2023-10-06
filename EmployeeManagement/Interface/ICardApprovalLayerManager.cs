using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    public interface ICardApprovalLayerManager
    {
        ICollection<CardApprovalLayer> GetAll();
        CardApprovalLayer GetById(int id);
        CardApprovalLayer GetbyEmpLid(int empLoyeeId);
        ICollection<CardApprovalLayer> GetAllbyTypeAndOrder(int order,int type);
    }
}
