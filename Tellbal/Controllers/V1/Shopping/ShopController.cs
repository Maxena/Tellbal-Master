using Common.Params;
using Common.Utilities;
using Entities.DTO;
using Entities.DTO.Product;
using Entities.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tellbal.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShopController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IManageService _manageService;

        public ShopController(IProductService productService, IManageService manageService)
        {
            _productService = productService;
            _manageService = manageService;
        }


        /// <summary>
        /// لیست محصولات ، سرچ ، فیلتر
        /// </summary>
        /// <remarks>
        /// بدون متن جستجو تمامی کالاها را شامل می شود
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("App/Products")]
        [HttpGet("Web/Products")]
        public async Task<ActionResult<PagedList<ProductToReturnDTO>>> SearchInProducts([FromQuery] PaginationParams<ProductSearch> pagination)
        {
            PagedList<ProductToReturnDTO> result = await _productService.SearchInProducts(pagination);

            return Ok(result);
        }

        /// <summary>
        /// لیست محصولات ، سرچ ، فیلتر
        /// </summary>
        /// <remarks>
        /// بدون متن جستجو تمامی کالاها را شامل می شود
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("Admin/Products")]
        public async Task<ActionResult<PagedList<ProductToReturnDTO>>> SearchInProductsForAdmin([FromQuery] PaginationParams<ProductSearch> pagination)
        {
            PagedList<ProductToReturnDTO> result = await _productService.SearchInProductsForAdmin(pagination);

            return Ok(result);
        }

        /// <summary>
        /// دریافت جزئیات یک محصول
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Admin/Products/{id}")]
        public ActionResult<ProductToReturnDTO> GetProductsForAdmin(Guid id)
        {
            ProductToReturnDTO dto = _productService.GetProductsForAdmin(id);

            return Ok(dto);
        }

        /// <summary>
        /// دریافت جزئیات یک محصول
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("App/Products/{id}")]
        [HttpGet("Web/Products/{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProducts(Guid id)
        {
            ProductToReturnDTO dto = await _productService.GetProducts(id);

            return Ok(dto);
        }
        /// <summary>
        /// دریافت کلید و مقدار های یک محصول
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("App/ProductKeysValues/{id}")]
        [HttpGet("Web/ProductKeysValues/{id}")]
        [HttpGet("Admin/ProductKeysValues/{id}")]
        public async Task<ActionResult<ProductKeysValuesToReturnDTO>> GetProductKeysAndValues(Guid id)
        {
            ProductKeysValuesToReturnDTO dto = await _productService.GetProductKeysAndValues(id);

            return Ok(dto);
        }

        /// <summary>
        /// مقایسه بین دو کالا
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        [HttpGet("App/CompareTwoProduct")]
        [HttpGet("Web/CompareTwoProduct")]
        public async Task<ActionResult<List<ProductToReturnDTO>>> CompareTwoProduct(List<Guid> productIds)
        {
            List<ProductToReturnDTO> ls = await _productService.CompareTwoProduct(productIds);

            return Ok(ls);
        }

        /// <summary>
        /// نمودار قیمت
        /// </summary>
        /// <remarks>
        /// for used products serve from the new one of this cat key val pair
        /// </remarks>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("App/ProductPriceLog")]
        [HttpGet("Web/ProductPriceLog")]
        public async Task<ActionResult<List<PriceLogDTO>>> ProductPriceLog(Guid productId)
        {
            List<PriceLogDTO> ls = await _productService.ProductPriceLog(productId);

            return Ok(ls);
        }

        /// <summary>
        /// نمودار هفتگی قیمت
        /// </summary>
        /// <remarks>
        /// for used products serve from the new one of this cat key val pair
        /// </remarks>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("App/ProductPriceLogOfWeek")]
        [HttpGet("Web/ProductPriceLogOfWeek")]
        public async Task<ActionResult<List<PriceLogDTO>>> ProductPriceLogOfWeek(Guid productId)
        {
            DateTime fromDT = DateTime.Now.AddDays(-7);
            DateTime toDT = DateTime.Now;

            List<PriceLogDTO> ls = await _productService.ProductPriceLogInDateRange(productId, fromDT, toDT);

            return Ok(ls);
        }

        /// <summary>
        /// نمودار ماهانه قیمت
        /// </summary>
        /// <remarks>
        /// for used products serve from the new one of this cat key val pair
        /// </remarks>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("App/ProductPriceLogOfMonth")]
        [HttpGet("Web/ProductPriceLogOfMonth")]
        public async Task<ActionResult<List<PriceLogDTO>>> ProductPriceLogOfMonth(Guid productId)
        {
            DateTime fromDT = DateTime.Now.AddDays(-30);
            DateTime toDT = DateTime.Now;

            List<PriceLogDTO> ls = await _productService.ProductPriceLogInDateRange(productId, fromDT, toDT);

            return Ok(ls);
        }

        /// <summary>
        /// لیست ویژگی های ساختار کالا
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        [HttpGet("Admin/PropertyKeys")]
        [HttpGet("App/PropertyKeys")]
        [HttpGet("Web/PropertyKeys")]
        public async Task<ActionResult<List<PropertyKeyDTO>>> GetPropertyKeys(int catId)
        {
            List<PropertyKeyDTO> ls = await _manageService.GetPropertyKeys(catId);

            return Ok(ls);
        }

        /// <summary>
        /// ثبت آگهی کالا
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Admin/Product")]
        public async Task<ActionResult<Guid>> AddProduct([FromBody] ProductForCreateDTO dto)
        {
            Guid id = await _productService.AddProduct(dto);
            return Ok(id);
        }

        /// <summary>
        /// ثبت یک عکس برای محصول
        /// </summary>
        /// <remarks>
        /// ابتدا عکس در سرور ذخیره و سپس با استفاده از آیدی در افزودن محصول استفاده شود
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Admin/Image")]
        public async Task<ActionResult<CreatedImageToReturnDTO>> AddImage(IFormFile Image)
        {
            CreatedImageToReturnDTO dto = await _productService.AddImage(Image);

            return Ok(dto);
        }

        /// <summary>
        /// حذف یک عکس از محصول
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Admin/Image/{id}")]
        public async Task<ActionResult<bool>> RemoveImage(Guid id)
        {
            bool res = await _productService.RemoveImage(id);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش کالا
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Admin/Product/{productId}")]
        public async Task<ActionResult<bool>> EditProduct(Guid productId, [FromBody] ProductForCreateDTO dto)
        {
            bool res = await _productService.EditProduct(productId, dto);

            return Ok(res);
        }
        /// <summary>
        /// تغییر استاتوس یک محصول
        /// </summary>
        /// <remarks>
        /// تغییر استاتوس به موجود ، ناموجود ، مخفی
        /// </remarks>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPatch("Admin/Product/{productId}")]
        public async Task<ActionResult<bool>> SetProductStatus(Guid productId, Status status)
        {
            bool res = await _productService.SetProductStatus(productId, status);

            return Ok(res);
        }

        //[HttpGet("App/BreadCrumbOfProduct")]
        //[HttpGet("Web/BreadCrumbOfProduct")]
        //public async Task<ActionResult<bool>> BreadCrumbOfProduct()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/SimilarSuggestions")]
        //[HttpGet("Web/SimilarSuggestions")]
        //public async Task<ActionResult<bool>> SimilarSuggestions()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/ProductKeyValue")]
        //[HttpGet("Web/ProductKeyValue")]
        //public async Task<ActionResult<bool>> ProductKeyValue()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/FiveProductKeyValue")]
        //[HttpGet("Web/FiveProductKeyValue")]
        //public async Task<ActionResult<bool>> FiveProductKeyValue()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        ///// <summary>
        ///// Get other entries of this product => based on same key val pair => multiple colors and etc.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("App/OtherEntriesOfOneProduct")]
        //[HttpGet("Web/OtherEntriesOfOneProduct")]
        //public async Task<ActionResult<bool>> OtherEntriesOfOneProduct()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/ProductImages")]
        //[HttpGet("Web/ProductImages")]
        //public async Task<ActionResult<bool>> ProductImages()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/AccessoriesOfProduct")]
        //[HttpGet("Web/AccessoriesOfProduct")]

        //public async Task<ActionResult<bool>> AccessoriesOfProduct()
        //{
        //    return Ok(Task.FromResult(true));
        //}
        ///// <summary>
        ///// Get price range by cat+brand+model
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("App/PriceRange")]
        //[HttpGet("Web/PriceRange")]
        //public async Task<ActionResult<bool>> PriceRange()
        //{
        //    return Ok(Task.FromResult(true));
        //}
        //[HttpGet("App/PaymentGatewaysList")]
        //[HttpGet("Web/PaymentGatewaysList")]
        //public async Task<ActionResult<bool>> PaymentGatewaysList()
        //{
        //    return Ok(Task.FromResult(true));
        //}

    }
}
