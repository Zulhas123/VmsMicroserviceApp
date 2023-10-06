using VisitorManagement.Enums;

namespace VisitorManagement.ViewModel
{
    public class VisitorApproveVm
    {    //visitor data 
        public string VisitorName { get; set; }
        public string VisitorMobileNo { get; set; }
        public string VisitDate { get; set; }
        public string Status { get; set; }
        //emp data
        public string ToWhom { get; set; }
        public string Department { get; set; }
        public string EmployeeCode { get; set; }
        public string? Purpose { get; set; }
    }
}
