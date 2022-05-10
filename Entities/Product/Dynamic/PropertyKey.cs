using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.Product.Dynamic
{
    public class PropertyKey : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public KeyType KeyType { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<PropertyValue> PropertyValues { get; set; }
    }
    public enum KeyType
    {
        /// <summary>
        /// عددی
        /// </summary>
        Numeric,
        /// <summary>
        /// منطقی
        /// </summary>
        Boolean,
        /// <summary>
        /// متنی
        /// </summary>
        Text
    }
    public class DynamicPropertyConfiguration : IEntityTypeConfiguration<PropertyKey>
    {
        public void Configure(EntityTypeBuilder<PropertyKey> builder)
        {
            builder.Property(p => p.CategoryId).IsRequired();
            builder.Property(p => p.KeyType)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<KeyType>(s));
        }
    }
}
