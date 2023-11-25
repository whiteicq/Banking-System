using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOModels
{
    public class CreditDTO
    {
        public int Id { get; set; }
        public decimal SumCredit { get; set; }
        public float InterestRate { get; set; }
        public int BankAccountId { get; set; }
        public DateTime CreditApprovalDate { get; set; }
        public int CreditTerm { get; set; }
        public CreditStatus Status { get; set; }
        public string Description { get; set; } = null!;
    }
}
