using CardManagement.Enums;

namespace CardManagement.ViewModel
{
    public class UserAccess
    {
        public int Id { get; set; }
        public string UHFCardNo { get; set; }
        public string HFCardNo { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set; }
        public UserType UserType { get; set; }
    }
}
