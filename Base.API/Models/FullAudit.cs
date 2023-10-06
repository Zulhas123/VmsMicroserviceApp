namespace Base.API.Models
{
    public class FullAudit:AuditCreateUpdate
    {
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
