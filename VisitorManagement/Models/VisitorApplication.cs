using Base.API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using VisitorManagement.Enums;

namespace VisitorManagement.Models
{
    public class VisitorApplication:FullAudit
    {
        public int Id { get; set; }    
        public int VisitorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string? OTP { get; set; }
        public Status Status { get; set; }
        public DateTime? IssueTime { get; set; }
        public string? QRCode { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public bool IsApprove { get; set; }
        public int ToWhom { get; set; }
        public string? Purpose { get; set; }
        public string?  NextApprovalEmpId { get; set; }
        public int  Order { get; set; }
        public string? ApprovedEmployeeId { get; set; }
        public bool IsFrontDesk { get; set; }
        public int? RejectedBy { get; set; }
    }
}
