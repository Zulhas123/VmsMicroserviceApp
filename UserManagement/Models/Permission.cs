using Base.API.Models;

namespace UserManagement.Models
{
    public class Permission:AuditCreate
    {
        public int Id { get; set; }
        public int RoleId  { get; set; }
        public int SubmenuId { get; set; }
    }
}
