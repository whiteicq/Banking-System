﻿using AutoMapper;
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

    public class BankAccountService : IFinancialOperations
    {
        private BankingDbContext _db;

        public BankAccountService(BankingDbContext dbContext)
        {
            _db = dbContext;
        }

        public decimal GetBalance(BankAccount bankAccount)
        {
            return bankAccount.Balance;
        }

        public Card CreateCard(BankAccount bankAccount)
        {
            if (bankAccount.IsFrozen)
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

            Card card = new Card()
            {
                CardNumber = cardNumber.ToString(),
                Cvv = cvv.ToString(),
                DateExpiration = DateTime.Now.AddYears(4),
                BankAccountId = bankAccount.Id
            };

            return card;
        }

        public void LinkCard(BankAccount bankAccount, Card card)
        {
            if (bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

            bankAccount.Cards.Add(card);
            _db.SaveChangesAsync();
        }

        // наверное передавать заявку Менеджеру надо
        public Credit TakeRequestCredit(BankAccount bankAccount, decimal sum, int term, string description = null!) // в конце метода должна быть оформлена заявка, а не сам кредит (далее дело за менеджером)
        {
            if (bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

            if (bankAccount.AccountType != BankAccountType.Credit)
            {
                throw new InvalidOperationException("Current bank account is not for credits");
            }

            Credit requestCredit = new Credit()
            {
                SumCredit = sum,
                InterestRate = 13.0f,
                CreditApprovalDate = DateTime.Now,
                BankAccountId = bankAccount.Id,
                CreditTerm = term,
                Status = CreditStatus.Active, // Для сдачи - Active, но по идее надо Question
                Description = description
            };

            bankAccount.Credits.Add(requestCredit);
            _db.SaveChanges();

            return requestCredit;
        }
       

        public void TakeTransaction(BankAccount senderBankAccount, BankAccount recipientBankAccount, decimal sum, string description = null!)
        {
            if (senderBankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Sender bank account is frozen");
            }

            if (recipientBankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Recipient bank account is frozen");
            }

            if (senderBankAccount.Currency == Currency.Dollar && recipientBankAccount.Currency == Currency.Ruble)
            {
                sum *= 3.30m;
            }

            if(senderBankAccount.Currency == Currency.Ruble && recipientBankAccount.Currency == Currency.Dollar)
            {
                sum /= 3.30m;
            }

            DataLayer.Entities.Transaction transactionForSender = new DataLayer.Entities.Transaction()
            {
                DateTransaction = DateTime.Now,
                SumTransaction = sum,
                BankAccountId = senderBankAccount.Id,
                Description = description,
                TransactionType = TransactionType.Outgoing
            };

            DataLayer.Entities.Transaction transactionForRecipient = new DataLayer.Entities.Transaction()
            {
                DateTransaction = DateTime.Now,
                SumTransaction = sum,
                BankAccountId = recipientBankAccount.Id,
                Description = description,
                TransactionType = TransactionType.Incoming
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                senderBankAccount.Balance -= sum;
                recipientBankAccount.Balance += sum;
                scope.Complete();
            }

            senderBankAccount.Transactions.Add(transactionForSender);
            recipientBankAccount.Transactions.Add(transactionForRecipient);
            _db.SaveChanges();
        }

        // бахнуть метод для внесения взноса за кредит? (мб связать с событиями (типа когда наступает срок выплаты по кредиту
        // вызывать событие на списание?))
        // Сделать СПЕЦ Счет для оплаты кредита (обычный счет, но будет передавать готовую сумму с учетом процентов)?

        // разовая оплата кредита (месячная)
        public void MakeCreditPayment(BankAccount bankAccount, Credit approvedCredit)
        {
            if (bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

            /*if (bankAccount.AccountType != BankAccountType.Credit)
            {
                throw new InvalidOperationException("Current bank account is not for credits");
            }

            Credit currentCredit = bankAccount.Credits.Find(credit => credit.Id == approvedCredit.Id);*/

            BankAccount systemBankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.Id == 20);

            if (approvedCredit.Status != CreditStatus.Active)
            {
                throw new InvalidOperationException("Current credit not approved");
            }

            decimal sumCreditPayment = GetSumOfMontlyPayment(approvedCredit.SumCredit, approvedCredit.InterestRate, approvedCredit.CreditTerm);

            TakeTransaction(bankAccount, systemBankAccount, sumCreditPayment, "Выплата по кредиту");
            _db.SaveChangesAsync();
        }

        // полная выплата кредита за раз
        public void RepayFullCredit(BankAccount bankAccount, Credit approvedCredit)
        {
            if (bankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

            /*if (bankAccount.AccountType != BankAccountType.Credit)
            {
                throw new InvalidOperationException("Current bank account is not for credits");
            }

            Credit currentCredit = bankAccount.Credits.Find(credit => credit.Id == approvedCredit.Id);*/

            if (approvedCredit is null)
            {
                throw new InvalidOperationException("Credit is not exist");
            }

            if (approvedCredit.Status != CreditStatus.Active)
            {
                throw new InvalidOperationException("Current credit not approved");
            }
            BankAccount systemBankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.Id == 20);

            decimal fullSum = GetFullSumOfCredit(approvedCredit.SumCredit, approvedCredit.InterestRate, approvedCredit.CreditTerm);

            TakeTransaction(bankAccount, systemBankAccount, fullSum, "Полная выплата кредита");

            approvedCredit.Status = CreditStatus.Closed;
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
