using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Identity
{
    /// <summary>
    /// Role Claim
    /// </summary>
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        /// <summary>
        /// Role
        /// </summary>
        public virtual Role Role { get; set; }
    }

    public class RoleClaimConfiguartion : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims");

            builder.HasIndex(e => e.RoleId);

            builder.HasOne(e => e.Role)
            .WithMany(f => f.RoleClaims)
            .HasForeignKey(k => k.RoleId);
        }
    }

}
