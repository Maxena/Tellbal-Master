using Entities.Product.Customers.DynamicPricing;
using Entities.Product.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Product
{
    public class Product : BaseEntity<Guid>
    {
        public string ProductName { get; set; }
        /// <summary>
        /// توضیحات کامل برای دستگاه های نو
        /// دستگاه های دست دوم معمولا توضیحات تکمیلی را ندارند
        /// </summary>
        public string Description { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// خلاصه وضعیت برای دستگاه های دست دوم
        /// خلاصه توضیحات برای دستگاه های نو هم میتواند باشد
        /// </summary>
        public string About { get; set; }
        public double Discount { get; set; }
        public string Code { get; set; }
        public ProductType ProductType { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<PriceLog> PriceLogs { get; set; }
        public ICollection<PropertyValue> PropertyValues { get; set; }
        public string Warranty { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Product> RelatedProducts { get; set; }
        public ICollection<HashTag> HashTags { get; set; }
        public ICollection<FastPricingDefinition> FastPricingDefinitions { get; set; }
        public Status Status { get; set; }
        public ICollection<Color> Colors { get; set; }
        /// <summary>
        /// متن توضیحات مهلت تست دستگاه
        /// </summary>
        public string ProductTestDescription { get; set; }


    }
    public enum ProductType
    {
        /// <summary>
        /// محصول نو
        /// </summary>
        New,

        /// <summary>
        /// محصول کارکرده
        /// </summary>
        Used,

        /// <summary>
        /// لوازم جانبی
        /// </summary>
        Accessory
    }
    public enum Status
    {
        /// <summary>
        /// محصول موجود
        /// </summary>
        Available,

        /// <summary>
        /// محصول ناموجود
        /// </summary>
        UnAvailable,

        /// <summary>
        /// مخفی از دید کاربر
        /// </summary>
        Hidden
    }
    public class ProductConfiguartion : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductName).HasMaxLength(100);

            builder.Property(p => p.ProductType)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<ProductType>(s));

            builder.Property(p => p.Status)
                .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<Status>(s));

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }
}
