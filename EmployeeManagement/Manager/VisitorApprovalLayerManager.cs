using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Enums;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class VisitorApprovalLayerManager : BaseManager<VisitorApprovalLayer>, IVisitorApprovalLayerManager
    {
        public VisitorApprovalLayerManager(DbContextClass db) : base(new BaseRepository<VisitorApprovalLayer>(db))
        {
        }

        public ICollection<VisitorApprovalLayer> GetAll()
        {
            return Get(c => true);
        }

        public VisitorApprovalLayer GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }

        public async Task<VisitorApprovalLayer> GetbyEmpLidAsync(int empLoyeeId)
        {
            return  GetFirstOrDefault(c => c.EmployeeId == empLoyeeId);
        }

        public async Task<ICollection<VisitorApprovalLayer>> GetAllbyOrderAsync(int order)
        {
            return  Get(c => c.IsActive && c.Order == order);
        }

        public async Task<ICollection<VisitorApprovalLayer>> GetbyVisitorLeyar(int departmentId, int order)
        {
            return Get(c => c.IsActive && c.DepartmemtId == departmentId && c.Order == order);
        }
    }

}
