using CardManagement.Enums;

namespace CardManagement.ViewModel
{
    public class CardResponse
    {
        public int Id { get;set; }
        public string Name { get;set; }
        public string? MobileNo { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Department { get; set; }
        public CardType Type { get; set; }
    }
}
