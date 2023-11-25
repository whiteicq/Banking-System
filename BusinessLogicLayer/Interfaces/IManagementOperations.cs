using BusinessLogicLayer.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IManagementOperations
    {
        void AcceptRequestCredit(CreditDTO requestCredit);
        void DeclineRequestCredit(CreditDTO requestCredit);
        void FreezeBankAccount(int bankAccountId);
        void UnFreezeBankAccount(int bankAccountId);
        IEnumerable<TransactionDTO> GetTransactions(BankAccountDTO bankAccount);
        IEnumerable<CreditDTO> GetCreditHistory(BankAccountDTO bankAccount);
    }
}
