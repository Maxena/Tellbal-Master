using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.System
{
    public class WhareHouse : BaseEntity<Guid>
    {
        public Product.Product Product { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public DateTime DT { get; set; }
        public WhareHouseState WhareHouseState { get; set; }

    }
    public enum WhareHouseState
    {
        /// <summary>
        /// ورود به انبار
        /// </summary>
        In,

        /// <summary>
        /// خروج از انبار
        /// </summary>
        Out,

        /// <summary>
        /// بازگشت
        /// </summary>
        Returned
    }
    public class WhareHouseConfiguartion : IEntityTypeConfiguration<WhareHouse>
    {
        public void Configure(EntityTypeBuilder<WhareHouse> builder)
        {
            builder.Property(p => p.WhareHouseState)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<WhareHouseState>(s));
        }
    }
}
