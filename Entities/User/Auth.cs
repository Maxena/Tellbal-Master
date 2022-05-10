using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.User
{
    public class Auth : BaseEntity<Guid>
    {
        public int Code { get; set; }
        public bool IsVerfied { get; set; } = false;
        public int Status { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public string Receptor { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
        public string StatusText { get; set; }
        public Guid? UserId { get; set; }
        public Identity.User User { get; set; }
    }
    public class AuthConfiguartion : IEntityTypeConfiguration<Auth>
    {
        public void Configure(EntityTypeBuilder<Auth> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(4);
            builder.Property(p => p.Receptor).IsRequired();
            builder.Property(p => p.Receptor).HasMaxLength(11);
            builder.Property(p => p.Sender).HasMaxLength(20);
        }

    }
}
