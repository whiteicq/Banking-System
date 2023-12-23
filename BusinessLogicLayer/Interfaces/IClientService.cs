using DataLayer;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IClientService
    {
        void CreateBankAccount(Account account, BankAccountType bankAccountType);
    }
}
