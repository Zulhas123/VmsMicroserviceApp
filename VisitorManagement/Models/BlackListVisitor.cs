using Base.API.Models;

namespace VisitorManagement.Models
{
    public class BlackListVisitor:AuditCreateUpdate
    {
        public int Id { get; set; }
        public int  VisitorId { get; set; }
        public string MobileNo { get; set; }

    }
}
