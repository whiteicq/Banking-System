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
        public IActionResult Registrate(string nickname, string email, string password, string confirmPassword, string phoneNumber, DateTime dateBirth)
        {
            if (password != confirmPassword)
            {
                return BadRequest("Password unconfirm");
            }

            _registrationService.CreateAccount(nickname, email, password, phoneNumber, dateBirth);
            return RedirectToAction("Login", "Login");
        }
    }
}
