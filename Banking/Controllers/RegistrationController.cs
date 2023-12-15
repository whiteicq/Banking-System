using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers
{
    public class RegistrationController : Controller
    {
        IRegistrationService _registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        public IActionResult Registrate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrate(string nickname, string email, string password, string phoneNumber, DateTime dateBirth)
        {
            _registrationService.CreateAccount(nickname, email, password, phoneNumber, dateBirth);
            return RedirectToAction("MyAccount", "Account");
        }
    }
}
