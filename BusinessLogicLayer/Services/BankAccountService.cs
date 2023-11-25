using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using System.Transactions;
using DataLayer.Entities;
using System.Text;
using DataLayer;

namespace BusinessLogicLayer.Services
{
    public class BankAccountService : IFInancialOperations
    {
        private BankAccountDTO _bankAccount;
        private BankingDbContext _db;
        private IMapper _mapper;
        // Добавить оператора (менеджера) в конструктор для подтверждения кредита 
        public BankAccountService(BankAccountDTO bankAccount, BankingDbContext dbContext, IMapper mapper)
        {
            _bankAccount = bankAccount;
            _db = dbContext;
            _mapper = mapper;
        }

        public decimal Balance
        {
            get
            {
               return _bankAccount.Balance;
            }

            set
            {
                if (value > 0)
                {
                    _bankAccount.Balance = value;
                }
            }
        }

        public CardDTO CreateCard()
        {
            Random random = new Random();
            StringBuilder cardNumber = new StringBuilder();
            while (cardNumber.Length < 16)
            {
                int rndNumber = random.Next(0, 10);
                cardNumber.Append(rndNumber);
            }

            StringBuilder cvv = new StringBuilder();
            while (cvv.Length < 3)
            {
                int rndCvv = random.Next(0, 10);
                cvv.Append(rndCvv);
            }

            CardDTO card = new CardDTO()
            {
                CardNumber = cardNumber.ToString(),
                Cvv = cvv.ToString(),
                DateExpiration = DateTime.Now.AddYears(4),
                BankAccountId = _bankAccount.Id,
                BankAccount = _bankAccount
            };

            return card;
        }

        public void LinkCard(CardDTO card)
        {
            _bankAccount.Cards.Add(card);
            var dalCard = _mapper.Map<Card>(card);
            _db.Cards.Add(dalCard);
            _db.SaveChanges();
        }

        // наверное передавать заявку Менеджеру надо
        public CreditDTO TakeRequestCredit(decimal sum, int term, string description = null!) // в конце метода должна быть оформлена заявка, а не сам кредит (далее делом за менеджером)
        {
            CreditDTO requestCredit = new CreditDTO()
            {
                SumCredit = sum,
                InterestRate = 13.0f,
                CreditApprovalDate = DateTime.Now,
                BankAccountId = _bankAccount.Id,
                CreditTerm = term,
                Status = CreditStatus.Question,
                Description = description
            };

            return requestCredit;
        }

        public void TakeTransaction(BankAccountDTO recipientBankAccount, decimal sum, string description = null!)
        {
            TransactionDTO transaction = new TransactionDTO()
            {
                DateTransaction = DateTime.Now,
                SumTransaction = sum,
                RecipientBankAccount = recipientBankAccount.Id,
                SenderBankAccount = _bankAccount.Id,
                Description = description,
                BankAccountId = _bankAccount.Id
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Balance -= sum;
                recipientBankAccount.Balance += sum;
                scope.Complete();
            }

            var DALtransaction = _mapper.Map<DataLayer.Entities.Transaction>(transaction);
            _db.Transactions.Add(DALtransaction);
            _db.SaveChanges();
            _bankAccount.Transactions.Add(transaction);
        }
    }
}
