using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.System
{
    public class Province : BaseEntity<Guid>
    {
        public Province()
        {
            Cities = new HashSet<City>();
        }

        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }

    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}
