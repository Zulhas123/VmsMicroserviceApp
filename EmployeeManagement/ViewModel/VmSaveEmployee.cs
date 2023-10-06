namespace EmployeeManagement.ViewModel
{
    public class VmSaveEmployee
    {
        public string EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public List<VmEmployeeData> Datalist { get; set; }
    }
}
