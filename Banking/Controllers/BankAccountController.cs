using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Interfaces;

namespace Banking.Controllers
{
    public class BankAccountController : Controller
    {
        IClientService _clientService;

        public BankAccountController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult CreateBankAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBankAccount(BankAccountType bankAccountType)
        {
            _clientService.CreateBankAccount(bankAccountType);
            return Ok();
        }
    }
}
