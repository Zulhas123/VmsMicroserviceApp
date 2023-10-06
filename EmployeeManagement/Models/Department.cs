using Base.API.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Department: FullAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; }

    }
}
