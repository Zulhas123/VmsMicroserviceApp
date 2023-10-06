using AccessManagement.Enums;
using Base.API.Models;

namespace AccessManagement.Models
{
    public class Gates:FullAudit
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string  ControllerSn { get; set; }
        public int DoorNo { get; set; }
        public UserType Type { get; set; }
    }
}
