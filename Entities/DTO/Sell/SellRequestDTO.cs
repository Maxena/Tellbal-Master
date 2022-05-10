using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Sell
{
    public class SellRequestDTO
    {
        public Guid AddressId { get; set; }
        public Guid DeviceId { get; set; }
    }
}
