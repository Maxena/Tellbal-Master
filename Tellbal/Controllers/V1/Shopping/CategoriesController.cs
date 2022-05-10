using AutoMapper;
using Data.Repositories;
using Entities.DTO;
using Entities.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Tellbal.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CategoriesController : ControllerBase
    {
        #region Configurations
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region APIs

        /// <summary>
        /// لیست انواع کالاها
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/RootCategories")]
        [HttpGet("Web/RootCategories")]
        [HttpGet("Admin/RootCategories")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> RootCategories()
        {
            List<CategoryToReturnDTO> ls = await _categoryService.RootCategories();

            return Ok(ls);
        }

        /// <summary>
        /// لیست تمامی برند های موجود
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/AllBrands")]
        [HttpGet("Web/AllBrands")]
        [HttpGet("Admin/AllBrands")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> AllBrands()
        {
            List<CategoryToReturnDTO> brands = await _categoryService.AllBrands();

            return Ok(brands);
        }
        /// <summary>
        /// لیست تمامی مدل های موجود
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/AllModels")]
        [HttpGet("Web/AllModels")]
        [HttpGet("Admin/AllModels")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> AllModels()
        {
            List<CategoryToReturnDTO> models = await _categoryService.AllModels();

            return Ok(models);
        }

        /// <summary>
        /// لیست برندهای یک نوع کالا
        /// </summary>
        /// <remarks>
        /// بر اساس نوع کالا
        /// </remarks>
        /// <param name="rootId"></param>
        /// <returns></returns>
        [HttpGet("App/BrandsOfCategory")]
        [HttpGet("Web/BrandsOfCategory")]
        [HttpGet("Admin/BrandsOfCategory")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> BrandsOfCategory([FromQuery] int rootId)
        {
            List<CategoryToReturnDTO> ls = await _categoryService.BrandsOfCategory(rootId);

            return Ok(ls);
        }

        /// <summary>
        /// لیست مدل های یک برند
        /// </summary>
        /// <remarks>
        /// بر اساس نوع برند
        /// </remarks>
        /// <param name="brandCategoryid"></param>
        /// <returns></returns>
        [HttpGet("App/ModelsOfBrand")]
        [HttpGet("Web/ModelsOfBrand")]
        [HttpGet("Admin/ModelsOfBrand")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> ModelsOfBrand([FromQuery] int brandCategoryid)
        {
            List<CategoryToReturnDTO> ls = await _categoryService.ModelsOfBrand(brandCategoryid);

            return Ok(ls);
        }

        /// <summary>
        /// ثبت یک دسته بندی
        /// </summary>
        /// <remarks>
        /// برای ثبت دسته بندی ریشه پرنت نال وارد شود
        /// </remarks>
        /// <param name="Name"></param>
        /// <param name="parentCategoryId"></param>
        /// <param name="Image"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Admin/Category")]
        public async Task<ActionResult<bool>> AddCategory(
            [Required][FromForm] string Name,
            [FromForm] int? parentCategoryId,
            [Required] IFormFile Image)
        {
            CategoryForSetDTO dto = new CategoryForSetDTO
            {
                Name = Name,
                ParentCategoryId = parentCategoryId,
                Img = Image
            };
            bool res = await _categoryService.AddCategory(dto);

            return Ok(res);
        }

        /// <summary>
        /// حذف یک دسته بندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Admin/Category/{id}")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            bool res = await _categoryService.DeleteCategory(id);

            return Ok(res);
        }

        /// <summary>
        /// آپدیت یک دسته بندی
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="Name"></param>
        /// <param name="parentCategoryId"></param>
        /// <param name="Image"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Admin/Category/{catId}")]
        public async Task<ActionResult<bool>> UpdateCategory(
            int catId,
            [Required][FromForm] string Name,
            [FromForm] int? parentCategoryId,
            [Required] IFormFile Image)
        {

            CategoryForSetDTO dto = new CategoryForSetDTO
            {
                Img = Image,
                Name = Name,
                ParentCategoryId = parentCategoryId
            };
            bool res = await _categoryService.UpdateCategory(catId, dto);

            return Ok(res);
        }

        /// <summary>
        /// تغییر ترتیب دسته بندی ها
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="arrangeIds"></param>
        /// <returns></returns>
        [HttpPut("Admin/Category/Arrange")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> CategoryArrange(int? parentId, List<int> arrangeIds)
        {
            List<CategoryToReturnDTO> ls = await _categoryService.CategoryArrange(parentId, arrangeIds);

            return Ok(ls);
        }

        /// <summary>
        /// دریافت همه دسته بندی های پدر یک دسته بندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Admin/Category/{id}/SubCategories")]
        [HttpGet("App/Category/{id}/SubCategories")]
        [HttpGet("Web/Category/{id}/SubCategories")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> GetSubCategories(int id)
        {
            List<CategoryToReturnDTO> ls = await _categoryService.GetSubCategories(id);

            return Ok(ls);
        }

        /// <summary>
        /// دریافت همه دسته بندی های فرزند یک دسته بندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Admin/Category/{id}/ParentCategories")]
        [HttpGet("App/Category/{id}/ParentCategories")]
        [HttpGet("Web/Category/{id}/ParentCategories")]
        public async Task<ActionResult<List<CategoryToReturnDTO>>> GetParentCategories(int id)
        {
            List<CategoryToReturnDTO> ls = await _categoryService.GetParentCategories(id);

            return Ok(ls);
        }

        #endregion

    }
}
