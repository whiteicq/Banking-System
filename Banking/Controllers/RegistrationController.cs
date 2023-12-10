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

        public IActionResult Registrate()
        {
            
            return View();
        }
    }
}
