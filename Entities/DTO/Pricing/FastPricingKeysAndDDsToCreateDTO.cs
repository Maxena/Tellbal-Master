using Entities.Product.Customers.DynamicPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class FastPricingKeysAndDDsToCreateDTO
    {
        public Guid? Id { get; set; }
        public string Section { get; set; }
        public string Name { get; set; }
        public string Hint { get; set; }
        public Entities.Product.Customers.DynamicPricing.ValueType ValueType { get; set; }
        public ICollection<FastPricingDDsToCreateDTO> FastPricingDDs { get; set; }

    }
}
