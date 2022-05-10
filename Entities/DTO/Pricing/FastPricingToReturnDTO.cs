using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class FastPricingToReturnDTO
    {
        public Guid DeviceId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string ImageUrl_L { get; set; }
        public string ImageUrl_M { get; set; }
        public string ImageUrl_S { get; set; }
        public DateTime DT { get; set; }
        public bool IsPriced { get; set; }
        public ICollection<FastPricingKeysAndDDsToReturnDTO> FastPricingKeys { get; set; }
    }
}
