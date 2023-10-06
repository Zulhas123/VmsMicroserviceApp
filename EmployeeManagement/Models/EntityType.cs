using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Base.API.Models;

namespace EmployeeManagement.Models
{
    public class EntityType: FullAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Entity Type is required")]
        [DisplayName("Entity Name")]
        public string Name { get; set; }
    }
}
