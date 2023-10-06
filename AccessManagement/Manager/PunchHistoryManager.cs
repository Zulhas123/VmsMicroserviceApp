using AccessManagement.Data;
using AccessManagement.Interface;
using AccessManagement.Models;
using Base.API.Manager;
using Base.API.Repository;
using System.Data;


namespace AccessManagement.Manager
{
    public class PunchHistoryManager : BaseManager<PunchHistory>, IPunchHistoryManager
    {
        public PunchHistoryManager(AppDbContext db) : base(new BaseRepository<PunchHistory>(db))
        {
        }
       
    }
}
