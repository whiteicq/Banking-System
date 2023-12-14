using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers
{
    // контроллер для моего аккаунта
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
