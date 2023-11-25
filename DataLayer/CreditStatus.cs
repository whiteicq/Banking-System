using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public enum CreditStatus
    {
        Active, // активный
        Closed, // закрытый (выплаченный)
        Question, // на рассмотрении
        Canceled, // не одобренный
        Default // не выполнены обязательства 
    }
}
