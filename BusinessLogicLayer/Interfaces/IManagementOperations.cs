using BusinessLogicLayer.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer;

namespace BusinessLogicLayer.Interfaces
{
    public interface IManagementOperations
    {
        void AcceptRequestCredit(Account manager,Credit requestCredit);
        void DeclineRequestCredit(Account manager, Credit requestCredit);
        void FreezeBankAccount(int bankAccountId);
        void UnFreezeBankAccount(int bankAccountId);
        IEnumerable<Transaction> GetTransactions(BankAccount bankAccount);
        IEnumerable<Credit> GetCreditHistory(BankAccount bankAccount);
    }
}
