using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.System
{
    public class Agency : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkingDays { get; set; } = "1_1_1_1_1_0_0";
        public TimeSpan WorkStart { get; set; }
        public TimeSpan WorkFinish { get; set; }
        public Identity.User User { get; set; }
        public Guid UserId { get; set; }
    }
    public class AgencyConfiguartion : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.HasOne(p => p.User)
                .WithOne(p => p.Agency)
                .HasForeignKey<Agency>(p => p.UserId);
        }
    }
}
