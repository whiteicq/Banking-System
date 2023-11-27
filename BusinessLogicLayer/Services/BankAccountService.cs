using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using System.Transactions;
using DataLayer.Entities;
using System.Text;
using DataLayer;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services
{
    // надо оформить норм комменты (///)
    // сделать главный Счет банка, с общим балансом,
    // с которого будет выдача денег на кредит и на который будут идти деньги с кредитов

    // добавить событие на приход/списание средств со счета, ответа менеджера по кредиту

    // При разработке сервисов для Клиента, Менеджера и тд, каждому сервис для своего Аккаунта делать (фабрика/фабричный метод) 
    // из-за Ролей

    public class BankAccountService : IFInancialOperations
    {
        private BankAccountDTO _bankAccount;
        private BankingDbContext _db;
        private IMapper _mapper;
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
            if (_bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

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
            if (_bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

            _bankAccount.Cards.Add(card);
            var dalCard = _mapper.Map<Card>(card);
            _db.Cards.Add(dalCard);
            _db.SaveChanges();
        }

        // наверное передавать заявку Менеджеру надо
        public CreditDTO TakeRequestCredit(decimal sum, int term, string description = null!) // в конце метода должна быть оформлена заявка, а не сам кредит (далее делом за менеджером)
        {
            if (_bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

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

            _bankAccount.Credits.Add(requestCredit);
            Credit credit = _mapper.Map<Credit>(requestCredit);
            _db.Credits.Add(credit);
            _db.SaveChanges();

            return requestCredit;
        }
        
        public void TakeTransaction(BankAccountDTO recipientBankAccount, decimal sum, string description = null!)
        {
            if (_bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

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
            _db.BankAccounts.Find(_bankAccount).Balance -= sum;
            _db.SaveChanges();

            var DALtransaction = _mapper.Map<DataLayer.Entities.Transaction>(transaction);
            _db.Transactions.Add(DALtransaction);
            _db.SaveChanges();
            _bankAccount.Transactions.Add(transaction);
        }

        // бахнуть метод для внесения взноса за кредит? (мб связать с событиями (типа когда наступает срок выплаты по кредиту
        // вызывать событие на списание?))
        // Сделать СПЕЦ Счет для оплаты кредита (обычный счет, но будет передавать готовую сумму с учетом процентов)?

        // разовая оплата кредита (месячная)
        public void MakeCreditPayment(CreditDTO approvedCredit)
        {
            CreditDTO currentCredit = _bankAccount.Credits.Find(credit => credit.Id == approvedCredit.Id);

            if (currentCredit.Status != CreditStatus.Active)
            {
                throw new InvalidOperationException("Current credit not approved");
            }

            decimal sumCreditPayment = GetSumOfMontlyPayment(currentCredit.SumCredit, currentCredit.InterestRate, currentCredit.CreditTerm);
            
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                // потом добавить зачисление на спец счет банка для кредитов
                Balance -= sumCreditPayment;
                scope.Complete();
            }

            _db.BankAccounts.Find(_bankAccount).Balance -= sumCreditPayment;
            _db.SaveChanges();
        }

        // полная выплата кредита за раз
        public void RepayFullCredit(CreditDTO approvedCredit)
        {
            CreditDTO currentCredit = _bankAccount.Credits.Find(credit => credit.Id == approvedCredit.Id);

            if (currentCredit.Status != CreditStatus.Active)
            {
                throw new InvalidOperationException("Current credit not approved");
            }

            decimal fullSum = GetFullSumOfCredit(currentCredit.SumCredit, currentCredit.InterestRate, currentCredit.CreditTerm);
            
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                // потом добавить зачисление на спец счет банка для кредитов
                Balance -= fullSum;
                scope.Complete();
            }

            _db.BankAccounts.Find(_bankAccount).Balance -= fullSum;
            _db.SaveChanges();
        }

        private decimal GetSumOfMontlyPayment(decimal sum, float percent, int term)
        {
            /*
            pmt - ежемесячный платеж
            p - сумма кредита
            r - месячная процентная ставка(годовая процентная ставка, деленная на 12)
            n - общее количество платежей(в месяцах)
            */
            decimal p = sum;
            float r = (percent / 100) / 12;
            int n = term * 12;

            decimal pmt = (p * (decimal)r * (decimal)Math.Pow(1 + r, n)) / (decimal)(Math.Pow(1 + r, n) - 1);
            return pmt;
        }

        private decimal GetFullSumOfCredit(decimal sum, float percent, int term)
        {
            decimal pmt = GetSumOfMontlyPayment(sum, percent, term);
            int n = term * 12;
            return pmt * n;
        }
    }
}
