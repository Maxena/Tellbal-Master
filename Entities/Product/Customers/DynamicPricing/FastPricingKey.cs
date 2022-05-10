using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.Product.Customers.DynamicPricing
{
    public class FastPricingKey : BaseEntity<Guid>
    {
        public FastPricingKey()
        {
            Id = Guid.NewGuid();
        }
        public string Section { get; set; }
        public string Name { get; set; }
        public string Hint { get; set; }
        public ValueType ValueType { get; set; }
        //public RateType? RateType { get; set; }
        public FastPricingDefinition FastPricingDefinition { get; set; }
        public Guid FastPricingDefinitionId { get; set; }
        public ICollection<FastPricingDD> FastPricingDDs { get; set; }
        public ICollection<FastPricingValue> fastPricingValues { get; set; }
    }
    public enum ValueType
    {
        /// <summary>
        /// منطقی
        /// بلی خیر
        /// </summary>
        Boolean,

        /// <summary>
        /// متنی
        /// </summary>
        Text,

        /// <summary>
        /// عددی
        /// </summary>
        Numeric
    }
    public enum RateType
    {
        /// <summary>
        /// تفریق
        /// </summary>
        Subtract,

        /// <summary>
        /// درصد
        /// </summary>
        Percent
    }

    public class FastPricingKeyConfiguration : IEntityTypeConfiguration<FastPricingKey>
    {
        public void Configure(EntityTypeBuilder<FastPricingKey> builder)
        {

            builder.Property(p => p.ValueType)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<ValueType>(s));

            //builder.Property(p => p.RateType)
            //    .HasConversion(
            //    e => e.ToString(),
            //    s => Enum.Parse<RateType>(s));
        }
    }
}
