using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
