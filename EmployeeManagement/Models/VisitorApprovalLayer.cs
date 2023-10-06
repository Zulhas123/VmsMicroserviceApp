using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Base.API.Models;

namespace EmployeeManagement.Models
{
    public class VisitorApprovalLayer: FullAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department is required")]
        [DisplayName("Department")]
        public int DepartmemtId { get; set; }
        [Required(ErrorMessage = "Employee is required")]
        [DisplayName("Employee")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Order is required")]
        public int Order { get; set; }
    }
}
