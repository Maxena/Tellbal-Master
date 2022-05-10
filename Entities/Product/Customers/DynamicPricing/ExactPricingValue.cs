using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product.Customers.DynamicPricing
{
    public class ExactPricingValue : BaseEntity<Guid>
    {
        public string Value { get; set; }
        public ExactPricingKey ExactPricingKey { get; set; }
        public Guid ExactPricingKeyId { get; set; }
        public Device Device { get; set; }

    }
    public class ExactPricingValueConfiguartion : IEntityTypeConfiguration<ExactPricingValue>
    {
        public void Configure(EntityTypeBuilder<ExactPricingValue> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ExactPricingKeyId).IsRequired();

            builder.HasOne(p => p.Device)
                .WithOne(p => p.ExactPricingValue)
                .HasForeignKey<ExactPricingValue>(p => p.Id);
        }

    }
}
