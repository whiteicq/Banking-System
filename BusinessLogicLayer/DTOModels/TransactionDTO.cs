using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOModels
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public DateTime DateTransaction { get; set; }
        public int RecipientBankAccount { get; set; }
        public int SenderBankAccount { get; set; }
        public decimal SumTransaction { get; set; }
        public string? Description { get; set; }
        public int BankAccountId { get; set; } 

        public TransactionDTO()
        {
            
        }
    }
}
