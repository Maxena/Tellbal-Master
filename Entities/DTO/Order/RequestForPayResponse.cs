using Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Order
{
    public class RequestForPayResponse
    {
        public string MerchantId { get; set; }
        public PaymentResponse PaymentResponse { get; set; }

    }
}
