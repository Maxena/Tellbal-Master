using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.User
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        //public Identity.User User { get; set; }
        public Identity.User User { get; set; }
        public Guid UserId { get; set; }
        public DateTime DT { get; set; }
        public Address Address { get; set; }
        public Guid? AddressId { get; set; }
        /// <summary>
        /// کد پیگیری 
        /// </summary>
        public string Code { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public TimeSpan Due { get; set; }
        public Coupon Coupon { get; set; }
        public Guid? CouponId { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
    public enum OrderStatus
    {
        /// <summary>
        /// سبد خرید
        /// </summary>
        Basket,

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
    public class OrderConfiguartion : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.OrderStatus)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<OrderStatus>(s));

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Tax).HasColumnType("decimal(18,2)");
            builder.Property(p => p.TotalPrice).HasColumnType("decimal(18,2)");

        }
    }
}
