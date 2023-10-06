namespace UserManagement.ViewModels
{
    public class ChangePasswordVm
    {
        //For Admi  reset
        public int UserId { get; set; }
        //For self reset
        public string Email { get; set; }
        //For Self change
        public string OldPassword { get; set; }
        //For reset or change
        public string NewPassword { get; set; }
        //For reset or change
        public string ConfirmPassword { get; set;}

    }
}
