using Common;
using Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Product
{
    public class ProductDetailToReturnDTO
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
        public Entities.Product.ProductType ProductType { get; set; }
        public List<ProductImageToReturnDTO> Images { get; set; }
        public List<ProductKeyToReturnDTO> KeyAndValues { get; set; }

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
        public int? brandId { get; set; }
    }
}
