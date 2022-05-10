using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product
{
    public class HashTag : BaseEntity<Guid>
    {
        public string Text { get; set; }
    }
    public class HashTagConfiguartion : IEntityTypeConfiguration<HashTag>
    {
        public void Configure(EntityTypeBuilder<HashTag> builder)
        {

        }
    }
}
