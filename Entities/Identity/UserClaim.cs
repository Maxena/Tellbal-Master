using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Identity
{
    /// <summary>
    /// UserClaim
    /// </summary>
    public class UserClaim : IdentityUserClaim<Guid>
    {
        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; }
    }

    public class UserClaimConfiguartion : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");

            builder.HasOne(e => e.User)
            .WithMany(f => f.Claims)
            .HasForeignKey(k => k.UserId);
        }
    }

}
