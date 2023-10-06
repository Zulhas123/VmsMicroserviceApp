using Base.API.Models;
using VisitorManagement.Enums;

namespace VisitorManagement.Models
{
    public class VisitorConfiguration:FullAudit
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public DataTypes DataType { get; set; }
        public bool IsRequired { get; set; }
    }
}
