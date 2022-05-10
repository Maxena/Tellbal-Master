using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product.Customers.DynamicPricing
{
    public class FastPricingValue : BaseEntity<Guid>
    {
        public FastPricingDD FastPricingDD { get; set; }
        public Guid FastPricingDDId { get; set; }
        public FastPricingKey FastPricingKey { get; set; }
        public Guid FastPricingKeyId { get; set; }
        public Device Device { get; set; }
        public Guid DeviceId { get; set; }
    }

    public class FastPricingValueConfiguartion : IEntityTypeConfiguration<FastPricingValue>
    {
        public void Configure(EntityTypeBuilder<FastPricingValue> builder)
        {
            builder.HasKey(p => p.Id);
        }

    }
}
