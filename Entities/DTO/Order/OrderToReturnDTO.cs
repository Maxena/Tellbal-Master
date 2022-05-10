using Entities.User;
using System;
using System.Collections.Generic;

namespace Entities.DTO
{
    public class OrderToReturnDTO
    {
        public Guid OrderId { get; set; }
        public DateTime DT { get; set; }
        public UserToReturnDTO User { get; set; }
        public AddressToReturnDTO Address { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatusDTO OrderStatus { get; set; }
        public string Code { get; set; }
        public double Discount { get; set; }
        public decimal Tax { get; set; }

    }
    public enum OrderStatusDTO
    {
        /// <summary>
        /// درحال بررسی
        /// </summary>
        Pending,

        /// <summary>
        /// در حال پردازش
        /// </summary>
        InProcess,

        /// <summary>
        /// تحویل به سفیر تل بال
        /// </summary>
        AgentDelivered,

        /// <summary>
        /// تحویل به مشتری
        /// </summary>
        CustomerDelivered,

        /// <summary>
        /// انصراف از فروش توسط فروشنده
        /// </summary>
        Canceled,

        /// <summary>
        /// مرجوعی
        /// </summary>
        Returned
    }
}
