using Common.Exceptions;
using Common.Utilities;
using Entities.DTO;
using Entities.DTO.Pricing;
using Entities.DTO.Sell;
using Entities.DTO.System;
using Entities.Product.Customers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tellbal.Controllers.V1.Management
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
    public class ManageController : ControllerBase
    {
        private readonly IManageService _manageService;
        private readonly IMemberService _memberService;

        public ManageController(
            IManageService manageService,
            IMemberService memberService)
        {
            _manageService = manageService;
            _memberService = memberService;
        }

        /// <summary>
        /// ایجاد ساختار کالا
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Admin/DefinePropertyKeys")]
        public async Task<ActionResult<bool>> DefinePropertyKeys([FromBody] ProductKeysDefinitionsDTO dto)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool res = await _manageService.DefinePropertyKes(dto);

            return Ok(res);
        }

        /// <summary>
        /// افزودن ویژگی به ساختار کالا
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Admin/AddToPropertyKeys")]
        public async Task<ActionResult<bool>> AddToPropertyKeys([FromBody] ProductKeysDefinitionsDTO dto)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool res = await _manageService.AddToPropertyKeys(dto);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش ویژگی های ساختار کالا
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("Admin/EditPropertyKeys")]
        public async Task<ActionResult<bool>> EditPropertyKeys([FromBody] List<PropertyKeyDTO> list)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool res = await _manageService.EditPropertyKeys(list);

            return Ok(res);
        }

        /// <summary>
        /// حذف ویژگی ها از ساختار کالا
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Admin/PropertyKeys/{id}")]
        public async Task<ActionResult<bool>> RemovePropertyKeys(Guid id)
        {
            bool res = await _manageService.RemovePropertyKeys(id);

            return Ok(res);
        }

        /// <summary>
        /// ایجاد یک ساختار قیمت گذاری
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Admin/DefineFastPricingKey")]
        public async Task<ActionResult<bool>> DefineFastPricingKey(FastPricingDefinitionToCreateDTO dto)
        {
            bool res = await _manageService.DefineFastPricingKey(dto);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش یک ساختار قیمت گذاری
        /// </summary>
        /// <returns></returns>
        [HttpPut("Admin/DefineFastPricing/{Id}")]
        public async Task<ActionResult<bool>> EditFastPricing(Guid Id, List<FastPricingKeysAndDDsToCreateDTO> ls)
        {
            bool res = await _manageService.EditFastPricing(Id, ls);

            return Ok(res);
        }

        /// <summary>
        /// دریافت لیست ساختارهای قیمت گذاری
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/FastPricingList")]
        public async Task<ActionResult<List<FastPricingDefinitionToReturnDTO>>> FastPricingList()
        {
            List<FastPricingDefinitionToReturnDTO> ls = await _manageService.FastPricingList();

            return Ok(ls);
        }
        /// <summary>
        /// دریافت یک ساختار قیمت گذاری
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Admin/FastPricing/{id}")]
        public async Task<ActionResult<List<FastPricingKeysAndDDsToReturnDTO>>> FastPricing(Guid id)
        {
            List<FastPricingKeysAndDDsToReturnDTO> ls = await _manageService.FastPricing(id);

            return Ok(ls);
        }

        /// <summary>
        /// حذف یک ساختار قیمت گذاری
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Admin/FastPricingDefinition/{id}")]
        public ActionResult<bool> RemoveFastPricingDefinition(Guid id)
        {
            bool res = _manageService.RemoveFastPricingDefinition(id);

            return Ok(res);
        }
    }
}
