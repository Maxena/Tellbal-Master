using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.Product.Customers.DynamicPricing
{
    public class FastPricingDD : BaseEntity<Guid>
    {
        public FastPricingDD()
        {
            Id = Guid.NewGuid();
        }
        public string Label { get; set; }
        public double? MinRate { get; set; }
        public double? MaxRate { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorDiscription { get; set; }
        public OperationType OperationType { get; set; }
        public FastPricingKey FastPricingKey { get; set; }
        public Guid? FastPricingKeyId { get; set; }
        public ICollection<FastPricingValue> FastPricingValues { get; set; }
    }
    public enum OperationType
    {
        /// <summary>
        /// عدم تاثیر در قیمت گذاری
        /// </summary>
        NoAffectOnPricing,

        /// <summary>
        /// عدم توانایی در قیمت گذاری
        /// </summary>
        ErrorOnPricing,

        /// <summary>
        /// قیمت گذاری با درصد
        /// </summary>
        PercentPricing
    }

    public class FastPricingDDConfiguration : IEntityTypeConfiguration<FastPricingDD>
    {
        public void Configure(EntityTypeBuilder<FastPricingDD> builder)
        {
            builder.Property(p => p.OperationType)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<OperationType>(s));
        }
    }
}
