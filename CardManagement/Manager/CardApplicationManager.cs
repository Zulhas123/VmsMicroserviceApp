using Base.API.Manager;
using Base.API.Repository;
using CardManagement.Data;
using CardManagement.Interface;
using CardManagement.Models;

namespace CardManagement.Manager
{
    public class CardApplicationManager:BaseManager<CardApplication>, ICardApplicationManager
    {
        public CardApplicationManager(ApplicationDbContext db) : base(new BaseRepository<CardApplication>(db))
        {
        }

        public CardApplication GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id && x.IsActive==true);
        }

        public IEnumerable<CardApplication> NextApprovalLayer(int orderId)
        {
            return Get(x => x.Order == orderId);
        }
    }
}
