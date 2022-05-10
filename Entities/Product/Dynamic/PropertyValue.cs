using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product.Dynamic
{
    public class PropertyValue : BaseEntity<Guid>
    {
        public string Value { get; set; }
        public PropertyKey PropertyKey { get; set; }
        public Guid PropertyKeyId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
    public class PropertyValueConfiguartion : IEntityTypeConfiguration<PropertyValue>
    {
        public void Configure(EntityTypeBuilder<PropertyValue> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PropertyKeyId).IsRequired();
        }

    }
}
