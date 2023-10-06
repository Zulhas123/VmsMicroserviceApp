using EmployeeManagement.Models;

namespace EmployeeManagement.Interface
{
    public interface IVisitorApprovalLayerManager
    {
        ICollection<VisitorApprovalLayer> GetAll();
        VisitorApprovalLayer GetById(int id);
        Task<VisitorApprovalLayer> GetbyEmpLidAsync(int empLoyeeId);
        Task<ICollection<VisitorApprovalLayer> GetbyVisitorLeyar(int departmentId,int order);
        Task<ICollection<VisitorApprovalLayer>> GetAllbyOrderAsync(int order);
    }

}
