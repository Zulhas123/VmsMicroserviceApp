using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class RegisterVm
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }
        public string? EmployeeCode { get; set; }
    }
}
