using Entities.Product;
using Entities.Product.Customers.DynamicPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class FastPricingForCreateDTO
    {
        public int CategoryId { get; set; }
        public ICollection<FastPricingValueToSetDTO> FastPricingValues { get; set; }
    }
}
