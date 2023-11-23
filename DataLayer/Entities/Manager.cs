using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Manager
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public DateTime DateBirth { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
