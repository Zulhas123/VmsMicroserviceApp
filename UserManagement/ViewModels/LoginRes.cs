namespace UserManagement.ViewModels
{
    public class LoginRes
    { 
        public string Token { get; set; }
        public List<PermissionVm> menus { get; set; }
    }
}
