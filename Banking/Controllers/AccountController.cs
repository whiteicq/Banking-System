using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Services;
using DataLayer.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;

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
        public IActionResult MyAccount() 
        {
            int userId = (int)TempData["userId"]!; // при обновлении страницы TempData очищается
            AccountDTO account = _mapper.Map<AccountDTO>(_db.Accounts.Find(userId)); 

            return View(account);
        }
    }
}
