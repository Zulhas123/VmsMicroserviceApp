using CardManagement.Enums;
using CardManagement.Models;

namespace CardManagement.ViewModel
{
    public class CardApplicationVM
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? ReferenceId { get; set; }
        public string? NextApprovalEmployee { get; set; }
        public string? Order { get; set; }
        public string? ApprovedEmployee { get; set; }
        public string? RejectedBy { get; set; }

        public string? IsPrinted { get; set; }
        public string? IsApprove { get; set; }
        public string? IsAccessGranted { get; set; }
    }
}
