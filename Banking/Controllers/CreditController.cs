using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration.Conventions;

namespace Banking.Controllers
{
    public class CreditController : Controller
    {
        IFinancialOperations _bankAccountService;
        BankingDbContext _db;

        public CreditController(IFinancialOperations bankAccountService, BankingDbContext db)
        {
            _bankAccountService = bankAccountService;
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult RequestCredit()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult TakeRequestCredit(string iban, decimal sum, int term, string description)
        {
            BankAccount bankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.IBAN == iban && ba.AccountType == BankAccountType.Credit);
            if (bankAccount is null)
            {
                return BadRequest();
            }

            Credit activeCredit = _bankAccountService.TakeRequestCredit(bankAccount, sum, term, description);

            bankAccount.Balance += activeCredit.SumCredit;
            _db.SaveChanges();
            // редирект на страницу кредитов мб
            return RedirectToAction("Credit", "Credit");
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult Credit()
        {
            string name = HttpContext.User.Identity.Name;

            Account acc = _db.Accounts
                .Include(acc => acc.BankAccounts)
                .ThenInclude(ba => ba.Credits)
                .FirstOrDefault(a => a.UserName == name);

            if (acc.UserName != name)
            {
                return Unauthorized();
            }

            return View(acc);
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult MakePaymentCredit()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult MakePaymentCredit(string iban, int creditId)
        {
            BankAccount bankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.IBAN == iban);
            Credit currentCredit = _db.Credits.FirstOrDefault(cr => cr.Id == creditId);

            _bankAccountService.MakeCreditPayment(bankAccount, currentCredit);

            return RedirectToAction("Credit", "Credit");
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult MakeFullPaymentCredit()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult MakeFullPaymentCredit(string iban, int creditId)
        {
            BankAccount bankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.IBAN == iban);
            Credit currentCredit = _db.Credits.FirstOrDefault(cr => cr.Id == creditId);

            _bankAccountService.RepayFullCredit(bankAccount, currentCredit);

            return RedirectToAction("Credit", "Credit");
        }
    }
}
