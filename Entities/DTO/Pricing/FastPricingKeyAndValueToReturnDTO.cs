using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class FastPricingKeyAndValueToReturnDTO
    {
        public Entities.Product.Customers.DynamicPricing.ValueType ValueType { get; set; }
        public string Section { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
