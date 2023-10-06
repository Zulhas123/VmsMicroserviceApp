using Base.API.Models;

namespace UserManagement.Models
{
    public class Submenu:AuditCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public string MenuName { get; set; }
        public string LabelName { get; set; }
        public string RouteValue { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        
    }
}
