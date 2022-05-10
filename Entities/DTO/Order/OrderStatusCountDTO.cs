using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class OrderStatusCountDTO
    {
        public OrderStatusDTO OrderStatus { get; set; }
        public int Count { get; set; }
    }
}
