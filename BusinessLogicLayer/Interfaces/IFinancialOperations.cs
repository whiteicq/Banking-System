using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTOModels;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFInancialOperations
    {
        void TakeTransaction(BankAccountDTO recipientBankAccount, decimal sum); // Совершить транзакцию
        CardDTO CreateCard(); // Открыть карту 
        void LinkCard(CardDTO card); // Привязка карты
        void TakeRequestCredit(); // Оформление кредита
        decimal Balance { get; set; } // Баланс счета
    }
}
