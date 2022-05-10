using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class OrderHistoryDTO
    {
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string PaymentName { get; set; }
        public bool TransactionStatus { get; set; }
        public string Code { get; set; }
    }
}
