using Base.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class Employee: FullAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Employee Code is required")]
        public string EmployeeCode { get; set; }
        [Required(ErrorMessage = "Employee name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Deparment is required")]
        public int  DepartmentId { get; set; }
        public string? signature { get; set; }
        public string? photo { get; set; }   
        [NotMapped]
        public string EmpNameCode { get; set; }

        public Employee()
        {
            EmpNameCode = EmployeeCode + "-" + Name;

        }
    }
}
