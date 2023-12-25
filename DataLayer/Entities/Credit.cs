using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Credit
    {
        public int Id { get; set; }
        public decimal SumCredit { get; set; }
        public DateTime CreditApprovalDate { get; set; }
        public int CreditTerm { get; set; }
        public float InterestRate { get; set; }
        [Column(TypeName = "varchar(20)")]
        public CreditStatus Status { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public string? Description { get; set; } = null!;
    }
}
