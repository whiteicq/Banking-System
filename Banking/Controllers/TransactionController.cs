using BusinessLogicLayer.Interfaces;
using DataLayer;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Transactions;

namespace Banking.Controllers
{
    public class TransactionController : Controller
    {
        private BankingDbContext _db;
        IFinancialOperations _bankAccountService;
        public TransactionController(BankingDbContext db, IFinancialOperations bankAccountService)
        {
            _db = db;
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public IActionResult Transaction()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TakeTransaction(string ibanSender, string ibanRecipient, decimal sum, string description)
        {
            if (ibanSender == ibanRecipient)
            {
                return BadRequest("Iban sender & iban recipient are equal");
            }

            var recipientBankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.IBAN == ibanRecipient);
            if (recipientBankAccount.AccountType != BankAccountType.Settlement || recipientBankAccount is null) // для кредитов потом придется убраться
            {
                return BadRequest();
            }

            var acc = _db.Accounts.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name);
            if (acc is null)
            {
                return BadRequest();
            }

            var senderBankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.AccountId == acc.Id && ba.IBAN == ibanSender);
            if (senderBankAccount.AccountType != BankAccountType.Settlement || recipientBankAccount is null)
            {
                return BadRequest();
            }

            _bankAccountService.TakeTransaction(senderBankAccount, recipientBankAccount, sum, description);
            return RedirectToAction("MyAccount", "Account", acc);
        }
    }
}
