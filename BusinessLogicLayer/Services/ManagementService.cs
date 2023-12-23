using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using DataLayer;
using DataLayer.Entities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

// рефакт
namespace BusinessLogicLayer.Services
{
    public class ManagementService : IManagementOperations
    {
        private BankingDbContext _db;
        public ManagementService(BankingDbContext dbContext)
        {
            /*if(AccountDTO)*/ // добавить проверку роли чтобы Акк был Манагером (ДТО исправить)
            _db = dbContext;
        }

        private bool IsManager(Account manager)
        {
            return manager.Role == Roles.Manager;
        }

        public void AcceptRequestCredit(Account manager, Credit requestCredit)
        {
            if (!IsManager(manager))
            {
                throw new Exception("This account is not manager");
            }

            if (requestCredit.Status == CreditStatus.Question)
            {
                BankAccount bankAccount = _db.BankAccounts.Find(requestCredit.BankAccountId)!;
                bankAccount.Credits.FirstOrDefault(credit => credit.Id == requestCredit.Id).Status = CreditStatus.Active;
                _db.SaveChanges();
                /*BankAccount acc = _bankAccounts.Find(c => c.Credits.Contains(requestCredit));
                acc.Credits.Find(credit => credit.Id == requestCredit.Id).Status = CreditStatus.Active;

                *//*Credit approveCredit = _db.Credits.Find(requestCredit.Id);
                approveCredit.Status = CreditStatus.Active;*//*
                _db.SaveChangesAsync();*/
            }
        }

        public void DeclineRequestCredit(Account manager, Credit requestCredit)
        {
            if (IsManager(manager))
            {
                throw new Exception("This account is not manager");
            }

            if (requestCredit.Status == CreditStatus.Question)
            {
                BankAccount bankAccount = _db.BankAccounts.Find(requestCredit.BankAccountId)!;
                bankAccount.Credits.FirstOrDefault(credit => credit.Id == requestCredit.Id).Status = CreditStatus.Canceled;
                _db.SaveChanges();
                /*Credit declineCredit = _db.Credits.Find(requestCredit.Id);
                declineCredit.Status = CreditStatus.Canceled;*/
            }
        }

        public void FreezeBankAccount(int bankAccountId)
        {
            BankAccount unFrozenBankAccount = _db.BankAccounts.Find(bankAccountId);
            unFrozenBankAccount.IsFrozen = true;
            _db.SaveChangesAsync();
        }

        public void UnFreezeBankAccount(int bankAccountId)
        {
            BankAccount frozenBankAccount = _db.BankAccounts.Find(bankAccountId);
            frozenBankAccount.IsFrozen = false;
            _db.SaveChangesAsync();
        }

        // по-хорошему, кредитную историю нужно смотреть конкретно по Аккаунту
        // (типо по всем его Счетам смотреть кредиты)
        public IEnumerable<Credit> GetCreditHistory(BankAccount bankAccount)
        {
            return bankAccount.Credits;
        }

        public IEnumerable<Transaction> GetTransactions(BankAccount bankAccount)
        {
            return bankAccount.Transactions;
        }
    }
}
