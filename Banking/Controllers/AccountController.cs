﻿using AutoMapper;
using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Services;
using DataLayer.EF;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using DataLayer;

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

            Account acc = _db.Accounts
                .Include(acc => acc.BankAccounts)
                .ThenInclude(ba => ba.Transactions)
                .FirstOrDefault(a => a.UserName == name);
            return View(acc);
        }
    }
}
