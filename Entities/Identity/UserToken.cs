using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Identity
{
    /// <summary>
    /// User Token
    /// </summary>
    public class UserToken : IdentityUserToken<Guid>
    {
        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; }
    }

    public class UserTokenConfiguartion : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens");

            builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            builder.HasOne(e => e.User)
                .WithMany(f => f.Tokens)
                .HasForeignKey(e => e.UserId);
        }
    }

}