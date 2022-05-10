using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Product.Customers.DynamicPricing
{
    public class FastPricingDefinition : BaseEntity<Guid>
    {
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<FastPricingKey> FastPricingKeys { get; set; }
    }
    public class FastPricingDefinitionConfiguration : IEntityTypeConfiguration<FastPricingDefinition>
    {
        public void Configure(EntityTypeBuilder<FastPricingDefinition> builder)
        {
            builder.HasOne(p => p.Category).WithOne(p => p.FastPricingDefinition)
                 .HasForeignKey<FastPricingDefinition>(p => p.CategoryId);

        }
    }
}
