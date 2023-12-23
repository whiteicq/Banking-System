using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Interfaces;
using DataLayer;
using DataLayer.EF;
using DataLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class ClientService : IClientService
    {
        private BankingDbContext _db;

        public ClientService(BankingDbContext db)
        {
            _db = db;
        }

        public void CreateBankAccount(Account account, BankAccountType bankAccountType)
        {
            BankAccount bankAccount = new BankAccount()
            {
                IBAN = GenerateIBAN(),
                DateCreate = DateTime.Now,
                Balance = 10000m,
                AccountType = bankAccountType,
                AccountId = account.Id,
                Account = account,
                Cards = new List<Card>(),
                Credits = new List<Credit>(),
                Transactions = new List<Transaction>(),
                IsFrozen = false,
            };

            if (account.BankAccounts is null)
            {
                account.BankAccounts = new List<BankAccount>();
            }

            account.BankAccounts.Add(bankAccount);
            _db.BankAccounts.AddAsync(bankAccount);
            _db.SaveChangesAsync();
        }

        private string GenerateIBAN()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 28; i++)
            {
                int r = random.Next(0, 10);
                sb.Append(r);
            }

            return sb.ToString();
        }
    }
}
