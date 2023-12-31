﻿using AutoMapper;
using BusinessLogicLayer.DTOModels;
using DataLayer;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mappings
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountDTO, Account>();

            CreateMap<BankAccount, BankAccountDTO>();
            CreateMap<BankAccountDTO, BankAccount>();

            CreateMap<Card, CardDTO>();
            CreateMap<CardDTO, Card>();

            CreateMap<Credit, CreditDTO>();
            CreateMap<CreditDTO, Credit>();

            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();


        }
    }
}
