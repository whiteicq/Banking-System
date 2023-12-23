using BusinessLogicLayer.Interfaces;
using DataLayer;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Banking.Controllers
{
    public class TransactionController : Controller
    {
        private BankingDbContext _db;
        /*IFInancialOperations _bankAccountService;*/
        public TransactionController(BankingDbContext db/*, IFInancialOperations bankAccountService*/)
        {
            _db = db;
            /*_bankAccountService = bankAccountService;*/
        }

        [HttpGet]
        public IActionResult Transaction()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TakeTransaction(string iban, decimal sum, string description)
        {
            var acc = _db.Accounts.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name);

            var sender = acc.BankAccounts.Last(); // для теста чисто
            var recipient = _db.BankAccounts.FirstOrDefault(ba => ba.IBAN == iban);
           
            if (recipient is null)
            {
                return BadRequest("Invalid IBAN");
            }

            customTakeTransaction(sender, recipient, sum, description);
            return RedirectToAction("MyAccount", "Account", sender.Account);
        }

        private void customTakeTransaction(BankAccount senderBankAccount, BankAccount recipientBankAccount, decimal sum, string description = null!)
        {
            if (senderBankAccount.IsFrozen)
            {
                throw new InvalidOperationException("Current bank account is frozen");
            }

            DataLayer.Entities.Transaction transaction = new DataLayer.Entities.Transaction()
            {
                DateTransaction = DateTime.Now,
                SumTransaction = sum,
                RecipientBankAccount = recipientBankAccount.Id,
                SenderBankAccount = senderBankAccount.Id,
                Description = description,
                BankAccountId = senderBankAccount.Id
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                senderBankAccount.Balance -= sum;
                recipientBankAccount.Balance += sum;
                scope.Complete();
            }

            senderBankAccount.Transactions.Add(transaction);
            _db.SaveChangesAsync();
        }
    }
}
