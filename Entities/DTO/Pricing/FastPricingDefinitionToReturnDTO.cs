using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Pricing
{
    public class FastPricingDefinitionToReturnDTO
    {
        public Guid Id { get; set; }
        public CategoryToReturnDTO Category { get; set; }
        public CategoryToReturnDTO Brand { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
