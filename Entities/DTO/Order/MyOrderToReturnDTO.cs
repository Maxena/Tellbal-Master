using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class MyOrderToReturnDTO
    {
        public Guid OrderId { get; set; }
        public string Code { get; set; }
        public DateTime DT { get; set; }
        public List<ProductInOrderToReturnDTO> ProductsInOrder { get; set; }
        public OrderStatusDTO OrderStatus { get; set; }
        public decimal Price { get; set; }
        public AddressToReturnDTO Address { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public int Count { get; set; }
    }
}
