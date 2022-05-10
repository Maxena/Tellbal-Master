using Entities.DTO.Order;
using Entities.Payment;
using Entities.User;
using System;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IPaymentService
    {
        Task<RequestForPayResponse> RequestForPay(Guid? paymentGateWayId, Order dbBasket);
        Task<VerificationResponse> VerifyPayment(Payment payment);
    }
}