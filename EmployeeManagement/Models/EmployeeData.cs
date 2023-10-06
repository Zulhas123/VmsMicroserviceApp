using Base.API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class EmployeeData: AuditCreate
    {
        public long Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmpConfigId { get; set; }
        public string Value { get; set; }

    }
}
