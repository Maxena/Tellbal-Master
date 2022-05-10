using Entities.DTO.Product;
using Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class ProductForCreateDTO
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
        public ProductType ProductType { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<PropertyValueForCreateDTO> PropertyValues { get; set; }
        public List<Guid> ImagesIds { get; set; }
        public Status Status { get; set; }
        public string ProductTestDescription { get; set; }
        public string Warranty { get; set; }
        public ICollection<ColorDTO> Colors { get; set; }
    }
}
