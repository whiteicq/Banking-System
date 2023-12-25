using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTOModels;
using DataLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFinancialOperations
    {
        decimal GetBalance(BankAccount bankAccount); // Баланс счета
        void TakeTransaction(BankAccount SenderBankAccount, BankAccount recipientBankAccount, decimal sum, string description); // Совершить транзакцию
        Card CreateCard(BankAccount bankAccount); // Открыть карту 
        void LinkCard(BankAccount bankAccount, Card card); // Привязка карты
        Credit TakeRequestCredit(BankAccount bankAccount, decimal sum, int term, string description); // Оформление кредита
        void MakeCreditPayment(BankAccount bankAccount, Credit approvedCredit); // Возмещение кредита за месяц
        void RepayFullCredit(BankAccount bankAccount, Credit approvedCredit); // Возмещение кредита целиком
    }
}
