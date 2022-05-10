using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.User
{
    public class Coupon : BaseEntity<Guid>
    {
        public Identity.User User { get; set; }
        public Guid UserId { get; set; }
        public TimeSpan Due { get; set; }
        public bool Used { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }

    }
    public class CouponConfiguartion : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        }
    }
}
