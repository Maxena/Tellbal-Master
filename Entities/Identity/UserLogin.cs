using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Identity
{
    /// <summary>
    /// User Login
    /// </summary>
    public class UserLogin : IdentityUserLogin<Guid>
    {
        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; }
    }

    public class UserLoginConfiguartion : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins");

            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            builder.HasOne(e => e.User)
            .WithMany(f => f.Logins)
            .HasForeignKey(k => k.UserId);
        }
    }

}