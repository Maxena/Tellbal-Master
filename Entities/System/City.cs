using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.System
{
    public class City : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public Province Province { get; set; }
        public Guid ProvinceId { get; set; }
    }
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}