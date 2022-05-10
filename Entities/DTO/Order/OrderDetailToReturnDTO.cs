using Entities.DTO.Product;
using Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class OrderDetailToReturnDTO
    {
        public ProductToReturnDTO Product { get; set; }
        public Guid ProductId { get; set; }
        public ColorDTO Color { get; set; }
        public Guid ColorId { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public double Discount { get; set; }
    }
}
