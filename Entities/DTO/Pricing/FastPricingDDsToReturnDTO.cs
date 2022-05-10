using Entities.Product.Customers.DynamicPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class FastPricingDDsToReturnDTO
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public double? MinRate { get; set; }
        public double? MaxRate { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorDiscription { get; set; }
        public OperationType OperationType { get; set; }
    }
}
