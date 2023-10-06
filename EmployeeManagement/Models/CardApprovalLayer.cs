using EmployeeManagement.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Base.API.Models;

namespace EmployeeManagement.Models
{
    public class CardApprovalLayer: FullAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Card Type is required")]
        [DisplayName("Card Type")]
        public CardApprovalType Type { get; set; }
        [Required(ErrorMessage = "Employee is required")]
        [DisplayName("Employee")]
        public int EmployeeId { get; set; }
        public int Order { get; set; }
    }
}
