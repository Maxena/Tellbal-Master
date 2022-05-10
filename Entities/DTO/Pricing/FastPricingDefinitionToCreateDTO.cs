using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Pricing
{
    public class FastPricingDefinitionToCreateDTO
    {
        public int CategoryId { get; set; }
        public Guid ProductId { get; set; }
        public ICollection<FastPricingKeysAndDDsToCreateDTO> FastPricingKeysAndDDs { get; set; }
    }
}
