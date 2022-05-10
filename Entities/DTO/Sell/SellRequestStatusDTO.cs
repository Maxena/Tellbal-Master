using Entities.Product.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Sell
{
    public class SellRequestStatusDTO
    {
        public string StatusDescription { get; set; }
        public SellRequestStatus SellRequestStatus { get; set; }
        public decimal AgreedPrice { get; set; }
    }
}
