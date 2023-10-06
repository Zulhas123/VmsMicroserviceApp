using Base.API.Models;
using CardManagement.Enums;

namespace CardManagement.Models
{
    public class CardHistory:AuditCreateUpdate
    {
        public int Id { get; set; }
        public CardType Type { get; set; }
        public int ReferenceId { get; set; }    
        public string HFCardNo { get; set; }
        public string UHFCardNo { get; set; }     
    }
}
