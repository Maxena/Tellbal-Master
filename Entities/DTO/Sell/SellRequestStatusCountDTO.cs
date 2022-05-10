using Entities.Product.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Sell
{
    public class SellRequestStatusCountDTO
    {
        public SellRequestStatus SellRequestStatus { get; set; }
        public int Count { get; set; }
    }
}
