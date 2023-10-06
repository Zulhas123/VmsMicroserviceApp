using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Base.API.Models;

namespace EmployeeManagement.Models
{
    public class EntityValue: FullAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Entity Type is required")]
        [DisplayName("Entity Type")]
        public int  EntityId { get; set; }
        [Required(ErrorMessage = "Entity Value is required")]
        [DisplayName("Entity Value")]
        public string Value { get; set; }
      
    }
}
