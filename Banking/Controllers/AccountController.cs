using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Services;
using DataLayer.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Controllers
{
    // контроллер для моего аккаунта
    public class AccountController : Controller
    {
        private BankingDbContext _db;
        private IMapper _mapper;
        public AccountController(BankingDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult MyAccount(Account account) 
        {
            string name = HttpContext.User.Identity.Name;
            if (!(account.UserName == name))
            {
                return Unauthorized();
            }
            
            return View(account);
        }
    }
}
