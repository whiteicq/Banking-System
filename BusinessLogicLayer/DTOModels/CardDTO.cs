using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOModels
{
    public class CardDTO
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = null!;
        public string Cvv { get; set; } = null!;
        public DateTime DateExpiration { get; set; }
        public int BankAccountId { get; set; }
        public BankAccountDTO BankAccount { get; set; } = null!;
    }
}
