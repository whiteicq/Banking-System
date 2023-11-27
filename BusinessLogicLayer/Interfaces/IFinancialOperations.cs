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
        decimal Balance { get; set; } // Баланс счета
        void TakeTransaction(BankAccountDTO recipientBankAccount, decimal sum, string description); // Совершить транзакцию
        CardDTO CreateCard(); // Открыть карту 
        void LinkCard(CardDTO card); // Привязка карты
        CreditDTO TakeRequestCredit(decimal sum, int term, string description); // Оформление кредита
        void MakeCreditPayment(CreditDTO approvedCredit); // Возмещение кредита за месяц
        void RepayFullCredit(CreditDTO approvedCredit); // Возмещение кредита целиком
    }
}
