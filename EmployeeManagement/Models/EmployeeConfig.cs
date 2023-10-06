using Base.API.Models;
using EmployeeManagement.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeConfig: AuditCreate
    {
        public int Id  { get; set; }
        [Required(ErrorMessage = "Property name is required")]
        [DisplayName("Property Name")]
        public string PropertyName { get; set; }
        [Required(ErrorMessage = "Data Type is required")]
        [DisplayName("Data Type")]
        public DataTypes DataType { get; set; }
        public bool  IsEntity { get; set; }
        public bool IsRequired { get; set; }
   
    }
}
