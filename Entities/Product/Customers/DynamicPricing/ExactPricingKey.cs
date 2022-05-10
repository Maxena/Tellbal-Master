using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product.Customers.DynamicPricing
{
    public class ExactPricingKey : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string ExactPricingKeyType { get; set; }
    }
    public class ExactPricingKeyConfiguration : IEntityTypeConfiguration<ExactPricingKey>
    {
        public void Configure(EntityTypeBuilder<ExactPricingKey> builder)
        {
        }
    }
}
