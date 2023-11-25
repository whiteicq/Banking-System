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
        void TakeTransaction(BankAccountDTO recipientBankAccount, decimal sum, string description); // Совершить транзакцию
        CardDTO CreateCard(); // Открыть карту 
        void LinkCard(CardDTO card); // Привязка карты
        CreditDTO TakeRequestCredit(decimal sum, int term, string description); // Оформление кредита
        decimal Balance { get; set; } // Баланс счета
    }
}
