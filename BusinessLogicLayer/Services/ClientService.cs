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
        private AccountDTO _account;
        private IMapper _mapper;

        public ClientService(AccountDTO account, BankingDbContext db, IMapper mapper)
        {
            _account = account;
            _db = db;
            _mapper = mapper;
        }

        public void CreateBankAccount(BankAccountType bankAccountType)
        {
            BankAccountDTO bankAccount = new BankAccountDTO()
            {
                DateCreate = DateTime.Now,
                Balance = 0m,
                AccountType = bankAccountType,
                AccountId = _account.Id,
                Account = _account,
                Cards = new List<CardDTO>(),
                Credits = new List<CreditDTO>(),
                Transactions = new List<TransactionDTO>(),
                IsFrozen = false,
            };

            _account.BankAccounts.Add(bankAccount);

            BankAccount ba = _mapper.Map<BankAccount>(bankAccount);
            _db.BankAccounts.Add(ba);
            _db.SaveChanges();
        }
    }
}
