using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Entities.Product
{
    public class Warranty : BaseEntity
    {
        public string Title { get; set; }
        public int Months { get; set; }
        public string Info { get; set; }
        public ICollection<Product> Products { get; set; }
    }
    public class WarrantyConfiguartion : IEntityTypeConfiguration<Warranty>
    {
        public void Configure(EntityTypeBuilder<Warranty> builder)
        {
            builder.HasKey(p => p.Id);
        }

    }
}
