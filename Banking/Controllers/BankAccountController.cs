using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using Microsoft.AspNetCore.Authorization;

namespace Banking.Controllers
{
    public class BankAccountController : Controller
    {
        private IClientService _clientService;
        private BankingDbContext _db;

        public BankAccountController(IClientService clientService, BankingDbContext db)
        {
            _clientService = clientService;
            _db = db;
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public IActionResult BankAccount()
        {
            return View();
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public IActionResult CreateBankAccount(BankAccountType bankAccountType)
        {
             var acc = _db.Accounts.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name);
             if (acc is null)
             {
                return BadRequest();
             }

            _clientService.CreateBankAccount(acc, bankAccountType);
            return RedirectToAction("MyAccount", "Account", acc);
        }
    }
}
