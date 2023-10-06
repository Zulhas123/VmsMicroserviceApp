namespace Base.API.Models
{
    public class AuditCreate
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
