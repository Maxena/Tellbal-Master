using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class BasketAddressDTO
    {
        public Guid AddressId { get; set; }
        public Guid BasketId { get; set; }
    }
}
