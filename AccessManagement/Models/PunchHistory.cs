namespace AccessManagement.Models
{
    public class PunchHistory
    {
        public long Id { get; set; }
        public string CardNo { get; set; }
        public DateTime Time { get; set; }
        public string IpAddress { get; set; }
        public string ControllerSn { get; set; }
        public int DoorNo { get; set; }
    }
}
