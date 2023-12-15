using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers
{
    public class BankAccountController : Controller
    {
        [HttpGet]
        public IActionResult CreateBankAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBankAccount(BankAccountType bankAccountType)
        {
            return Ok();
        }
    }
}
