namespace UserManagement.ViewModels
{
    public class UserInfoVm
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }     
        public string? EmployeeCode { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
