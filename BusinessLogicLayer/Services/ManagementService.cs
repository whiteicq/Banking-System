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

namespace BusinessLogicLayer.Services
{
    public class ManagementService : IManagementOperations
    {
        private ManagerDTO _manager;
        private BankingDbContext _db;
        private IMapper _mapper;
        private List<BankAccountDTO> _bankAccounts;
        public ManagementService(ManagerDTO manager, BankingDbContext dbContext, IMapper mapper)
        {
            _manager = manager;
            _db = dbContext;
            _mapper = mapper;
            
            foreach (var ba in _db.BankAccounts)
            {
                _bankAccounts.Add(_mapper.Map<BankAccountDTO>(ba));
            }
        }

        public void AcceptRequestCredit(CreditDTO requestCredit)
        {
            if (requestCredit.Status == CreditStatus.Question)
            {
                BankAccountDTO acc = _bankAccounts.Find(c => c.Credits.Contains(requestCredit));
                acc.Credits.Find(credit => credit.Id == requestCredit.Id).Status = CreditStatus.Active;

                Credit approveCredit = _db.Credits.Find(requestCredit.Id);
                approveCredit.Status = CreditStatus.Active;
                _db.SaveChanges();
            }
        }

        public void DeclineRequestCredit(CreditDTO requestCredit)
        {
            if (requestCredit.Status == CreditStatus.Question)
            {
                BankAccountDTO acc = _bankAccounts.Find(c => c.Credits.Contains(requestCredit));
                acc.Credits.Find(credit => credit.Id == requestCredit.Id).Status = CreditStatus.Active;

                Credit declineCredit = _db.Credits.Find(requestCredit.Id);
                declineCredit.Status = CreditStatus.Canceled;
                _db.SaveChanges();
            }
        }

        public void FreezeBankAccount(int bankAccountId)
        {
            BankAccountDTO unFrozenBankAccount = _bankAccounts.Find(ba => ba.Id == bankAccountId);
            unFrozenBankAccount.IsFrozen = true;

            BankAccount dbUnFrozenBankAccount = _db.BankAccounts.Find(bankAccountId);
            dbUnFrozenBankAccount.IsFrozen = true;
            _db.SaveChanges();
        }

        public void UnFreezeBankAccount(int bankAccountId)
        {
            BankAccountDTO frozenBankAccount = _bankAccounts.Find(ba => ba.Id == bankAccountId);
            frozenBankAccount.IsFrozen = false;

            BankAccount dbFrozenBankAccount = _db.BankAccounts.Find(bankAccountId);
            dbFrozenBankAccount.IsFrozen = true;
            _db.SaveChanges();
        }

        // по хорошему, кредитную историю нужно смотреть конкретно по Аккаунту
        // (типо по всем его Счетам смотреть кредиты)
        public IEnumerable<CreditDTO> GetCreditHistory(BankAccountDTO bankAccount)
        {
            return bankAccount.Credits;
        }

        public IEnumerable<TransactionDTO> GetTransactions(BankAccountDTO bankAccount)
        {
            return bankAccount.Transactions;
        }
    }
}
