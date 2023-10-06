namespace UserManagement.Models
{
    public class ForgetPassword
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime ValidTill { get; set; }
    }
}
