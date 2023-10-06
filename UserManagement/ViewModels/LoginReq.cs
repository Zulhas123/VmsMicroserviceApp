using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class LoginReq
    {
        [Required(ErrorMessage ="User name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }        
        
    }
}
