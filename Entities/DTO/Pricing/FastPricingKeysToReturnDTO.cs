using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Pricing
{
    public class FastPricingKeysToReturnDTO
    {
        public Guid FastPricingKeyId { get; set; }
        public string Section { get; set; }
        public string Name { get; set; }
        public string Hint { get; set; }
        public Entities.Product.Customers.DynamicPricing.ValueType ValueType { get; set; }
        public FastPricingDDsToReturnDTO FastPricingDD { get; set; }
    }
}
