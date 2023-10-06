using Base.API.Interface.Manager;
using CardManagement.Enums;
using CardManagement.Models;


namespace CardManagement.Interface
{
    interface ICardHistoryManager : IBaseManager<CardHistory>
    {
        CardHistory AddCardHistory(int referenceId, CardType type);
    }
}
