using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product
{
    public class PriceLog : BaseEntity<Guid>
    {
        public decimal Price { get; set; }
        public DateTime DT { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
    }
    public class PriceAverageConfiguartion : IEntityTypeConfiguration<PriceLog>
    {
        public void Configure(EntityTypeBuilder<PriceLog> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }

}
