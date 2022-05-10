using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.System
{
    public class Consulting : BaseEntity<Guid>
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public ConsultingStatus ConsultingStatus { get; set; }
        public DateTime DT { get; set; }
        public Identity.User User { get; set; }
        public Guid UserId { get; set; }

    }
    public enum ConsultingStatus
    {
        /// <summary>
        /// دیده نشده
        /// </summary>
        NotSeened,

        /// <summary>
        /// دیده شده
        /// </summary>
        Seened,

        /// <summary>
        /// درحال بررسی
        /// </summary>
        InProcess,

        /// <summary>
        /// بسته شده
        /// </summary>
        Closed
    }
    public class ConsultingConfiguartion : IEntityTypeConfiguration<Consulting>
    {
        public void Configure(EntityTypeBuilder<Consulting> builder)
        {
            builder.Property(p => p.ConsultingStatus)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<ConsultingStatus>(s));
        }
    }
}
