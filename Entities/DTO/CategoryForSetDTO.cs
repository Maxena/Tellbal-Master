using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class CategoryForSetDTO
    {
        public IFormFile Img { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
