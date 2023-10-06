using Base.API.Manager;
using Base.API.Repository;
using CardManagement.Data;
using CardManagement.Enums;
using CardManagement.Interface;
using CardManagement.Models;

namespace CardManagement.Manager
{
    public class CardHistoryManager : BaseManager<CardHistory>, ICardHistoryManager
    {
        public CardHistoryManager(ApplicationDbContext db) : base(new BaseRepository<CardHistory>(db))
        {
        }

        public CardHistory AddCardHistory(int referenceId, CardType type)
        {
            return GetFirstOrDefault(c => c.IsActive == true && c.ReferenceId == referenceId && c.Type == type);
        }
    }
}
