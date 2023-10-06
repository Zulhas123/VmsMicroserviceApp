using Base.API.Models;

namespace UserManagement.Models
{
    public class Role:FullAudit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
    }
}
