using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;
using DataLayer;

namespace Banking.Controllers
{
    public class RequestCreditController : Controller
    {
        IFinancialOperations _bankAccountService;
        BankingDbContext _db;

        public RequestCreditController(IFinancialOperations bankAccountService, BankingDbContext db)
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
        public IActionResult TakeRequstCredit(string iban, decimal sum, int term, string description)
        {
            BankAccount bankAccount = _db.BankAccounts.FirstOrDefault(ba => ba.IBAN == iban && ba.AccountType == BankAccountType.Credit);
            if (bankAccount is null)
            {
                return BadRequest();
            }

            _bankAccountService.TakeRequestCredit(bankAccount, sum, term, description);
            // редирект на страницу кредитов мб
            return Ok();
        }
    }
}
