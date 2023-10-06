using VisitorManagement.Models;

namespace VisitorManagement.ViewModel
{
    public class SaveVisitorVm
    {
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public DateTime VisitDate { get; set; }
        public int ToWhom { get; set; }
        public string? Purpose { get; set; }
        public bool IsSelfRegister { get; set; }
        public List<VisitorDataVM>? VisitorData { get; set; }
    }
}
