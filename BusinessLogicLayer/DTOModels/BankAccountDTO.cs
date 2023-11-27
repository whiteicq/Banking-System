using DataLayer.Entities;
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOModels
{
    public class BankAccountDTO
    {
        public int Id { get; set; }
        public BankAccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateCreate { get; set; }
        public AccountDTO Account { get; set; } = null!;
        public int AccountId { get; set; }
        public bool IsFrozen { get; set; } = false;
        public List<CardDTO> Cards { get; set; } = null!;
        public List<CreditDTO> Credits { get; set; } = null!;
        public List<TransactionDTO> Transactions { get; set; } = null!;
    }
}
