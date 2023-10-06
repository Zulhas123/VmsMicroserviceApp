namespace Base.API.Models
{
    public class AuditCreateUpdate:AuditCreate
    {
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
