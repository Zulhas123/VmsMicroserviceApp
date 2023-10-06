using Base.API.Manager;
using Base.API.Repository;
using EmployeeManagement.Data;
using EmployeeManagement.Enums;
using EmployeeManagement.Interface;
using EmployeeManagement.Models;

namespace EmployeeManagement.Manager
{
    public class CardApprovalLayerManager : BaseManager<CardApprovalLayer>, ICardApprovalLayerManager
    {
        public CardApprovalLayerManager(DbContextClass db) : base(new BaseRepository<CardApprovalLayer>(db))
        {
        }

        public ICollection<CardApprovalLayer> GetAll()
        {
            return Get(c => true);
        }

        public CardApprovalLayer GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
        public CardApprovalLayer GetbyEmpLid(int empLoyeeId)
        {
            return GetFirstOrDefault(c => c.EmployeeId == empLoyeeId);
        }

        public ICollection<CardApprovalLayer> GetAllbyTypeAndOrder( int order,int type)
        {
            var enumType = (CardApprovalType)type; 

            return Get(c => c.IsActive && c.Order == order && c.Type== enumType);
        }

    }
}
