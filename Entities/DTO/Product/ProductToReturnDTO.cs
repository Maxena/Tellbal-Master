using Entities.DTO.Product;
using Entities.Product;
using System;
using System.Collections.Generic;

namespace Entities.DTO
{
    public class ProductToReturnDTO
    {
        public Guid ProductId { get; set; }
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
        public List<ProductImageToReturnDTO> Images { get; set; }
        public List<PropertyKeyDTO> PropertyKeys { get; set; }
        public List<PropertyValueDTO> PropertyValues { get; set; }
        /// <summary>
        /// موجود - ناموجود - مخفی
        /// </summary>
        public Status? Status { get; set; }
        public string Warranty { get; set; }
        /// <summary>
        /// متن توضیحات مهلت تست دستگاه
        /// </summary>
        public string ProductTestDescription { get; set; }
        public ICollection<ColorDTO> Colors { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }

    }
}
