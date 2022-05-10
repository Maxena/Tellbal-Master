using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product.Customers
{
    public class ExchangeRequest : BaseEntity<Guid>
    {
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public Device Device { get; set; }
    }
    public class ExchangeRequestConfiguartion : IEntityTypeConfiguration<ExchangeRequest>
    {
        public void Configure(EntityTypeBuilder<ExchangeRequest> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Device)
                .WithOne(p => p.ExchangeRequest)
                .HasForeignKey<ExchangeRequest>(p => p.Id);
        }
    }
}
