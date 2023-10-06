 using Base.API.Models;
using CardManagement.Enums;

namespace CardManagement.Models
{
    public class CardApplication:FullAudit
    {
        public int Id { get; set; }
        public CardType Type { get; set; }
        public int ReferenceId { get; set; }
        public string? NextApprovalEmployee { get; set; }
        public int Order { get; set; }
        public string? ApprovedEmployee { get; set; }
        public int? RejectedBy { get; set; }


        public bool IsPrinted { get; set; }
        public bool IsApprove { get; set; }
        public bool IsAccessGranted { get; set; }
    }
}
