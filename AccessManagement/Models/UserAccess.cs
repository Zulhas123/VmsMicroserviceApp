using AccessManagement.Enums;
using Base.API.Models;

namespace AccessManagement.Models
{
    public class UserAccess:FullAudit
    {
        public long Id { get; set; }
        public string UHFCardNo { get; set; }
        public string HFCardNo { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set;}
        public UserType UserType { get; set; }
    }
}
