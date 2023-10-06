using Base.API.Models;

namespace VisitorManagement.Models
{
    public class Visitor:AuditCreateUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string? Photo { get; set; }
        public string? Signature { get; set; }
        //public VisitorConfiguration Configuration { get; set; }
        //public int ConfigurationId { get; set; }
    }
}
