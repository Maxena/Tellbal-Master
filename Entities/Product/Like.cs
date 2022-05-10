using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Product
{
    public class Like
    {
        public Guid UserId { get; set; }
        public Identity.User User { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
    public class LikeConfiguartion : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.UserId });
        }
    }
}
