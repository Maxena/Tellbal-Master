using Entities.DTO.Pricing;
using Entities.Product.Customers;
using Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Sell
{
    public class SellRequestToReturnDTO
    {
        public Guid Id { get; set; }
        public DateTime DT { get; set; }
        public string Code { get; set; }
        public string StatusDescription { get; set; }
        public SellRequestStatus SellRequestStatus { get; set; }
        public decimal AgreedPrice { get; set; }
        public CategoryToReturnDTO Category { get; set; }
        public CategoryToReturnDTO Model { get; set; }
        public CategoryToReturnDTO Brand { get; set; }
        public UserToReturnDTO User { get; set; }
        //public List<FastPricingKeysToReturnDTO> FastPricingKeys { get; set; }
        public AddressToReturnDTO Address { get; set; }
    }
}
