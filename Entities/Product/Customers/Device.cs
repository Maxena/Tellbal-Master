using Entities.Product.Customers.DynamicPricing;
using Entities.Product.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.Product.Customers
{
    public class Device : BaseEntity<Guid>
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public bool IsPriced { get; set; } = false;
        public SellRequest SellRequest { get; set; }
        public ExchangeRequest ExchangeRequest { get; set; }
        public RepairRequest RepairRequest { get; set; }
        public Guid UserId { get; set; }
        public Identity.User User { get; set; }
        public ExactPricingValue ExactPricingValue { get; set; }
        public ICollection<FastPricingValue> FastPricingValues { get; set; }

    }
    public class DeviceConfiguartion : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
        }
    }
}
