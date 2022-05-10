using Common;
using Data;
using Entities.DTO.Order;
using Entities.Payment;
using Entities.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly SiteSettings _siteSettings;
        private readonly ApplicationDbContext _dbContext;
        public PaymentService(IOptionsSnapshot<SiteSettings> siteSettings, ApplicationDbContext dbContext)
        {
            _siteSettings = siteSettings.Value;
            _dbContext = dbContext;
        }

        public async Task<RequestForPayResponse> RequestForPay(Guid? paymentGateWayId, Order dbBasket)
        {
            var paymentRequest = new PaymentRequest(
                _siteSettings.PaymentSettings.ZarinMerchantId,
                (long)dbBasket.Price,
                _siteSettings.PaymentSettings.CallBackUrl,
                "پرداخت سبد خرید تل بال");


            URLs url = new URLs(true);

            var _HttpCore = new HttpCore();
            _HttpCore.URL = url.GetPaymentRequestURL();
            _HttpCore.Method = Method.POST;
            _HttpCore.Raw = paymentRequest;


            String response = _HttpCore.Get();

            PaymentResponse _Response = JsonSerializer.Deserialize<PaymentResponse>(response);
            _Response.PaymentURL = url.GetPaymenGatewayURL(_Response.Authority);

            return new RequestForPayResponse
            {
                MerchantId = _siteSettings.PaymentSettings.ZarinMerchantId,
                PaymentResponse = _Response
            };
        }

        public async Task<VerificationResponse> VerifyPayment(Payment payment)
        {
            URLs url = new URLs(true,true);
            var _HttpCore = new HttpCore();
            _HttpCore.URL = url.GetVerificationURL();
            _HttpCore.Method = Method.POST;

            var verificationRequest = new PaymentVerification(payment.MerchantID, (long)payment.Order.Price, payment.Authority);

            _HttpCore.Raw = verificationRequest;


            String response = _HttpCore.Get();
            //JavaScriptSerializer j = new JavaScriptSerializer();
            VerificationResponse verification = JsonSerializer.Deserialize<VerificationResponse>(response);

            return verification;
        }
    }
}
