using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class BasketCheckOutDTO
    {
        public Guid BasketId { get; set; }
        public Guid? PaymentGateWayId { get; set; }
    }
}
