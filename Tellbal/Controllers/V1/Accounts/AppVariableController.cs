using Entities.DTO;
using Entities.DTO.System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tellbal.Controllers.V1.Accounts
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppVariableController : ControllerBase
    {
        private readonly IManageService _manageService;
        private readonly IMemberService _memberService;
        public AppVariableController(IManageService manageService, IMemberService memberService)
        {
            _manageService = manageService;
            _memberService = memberService;
        }

        /// <summary>
        /// افزودن سوال متداول جدید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Admin/FAQ")]
        public async Task<ActionResult<bool>> FAQ(FAQToCreateDTO dto)
        {
            bool res = await _manageService.FAQ(dto);

            return Ok(res);
        }
        /// <summary>
        /// مرتب کردن سوالات متداول
        /// </summary>
        /// <param name="arrangeIds"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Admin/FAQ/Arrange")]
        public async Task<ActionResult<FAQToReturnDTO>> ArrangeFAQs(List<int> arrangeIds)
        {
            List<FAQToReturnDTO> ls = await _manageService.ArrangeFAQs(arrangeIds);

            return Ok(ls);
        }

        /// <summary>
        /// حذف یک سوال متدوال
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Admin/FAQ")]
        public async Task<ActionResult<bool>> RemoveFAQ(Guid id)
        {
            bool res = await _manageService.RemoveFAQ(id);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش درباره ما
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Admin/AboutUs")]
        public async Task<ActionResult<bool>> AboutUs(AppVariableDTO dto)
        {
            bool res = await _manageService.UpdateAboutUs(dto);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش حریم خصوصی و امنیت
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Admin/SecurityAndPrivacy")]
        public async Task<ActionResult<bool>> SecurityAndPrivacy(AppVariableDTO dto)
        {
            bool res = await _manageService.SecurityAndPrivacy(dto);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش شرایط وضوابط
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Admin/TermsAndConditions")]
        public async Task<ActionResult<bool>> TermsAndCondition(AppVariableDTO dto)
        {
            bool res = await _manageService.TermsAndCondition(dto);

            return Ok(res);
        }
        /// <summary>
        /// لیست سوالات متدوال
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/FAQ")]
        [HttpGet("App/FAQ")]
        [HttpGet("Web/FAQ")]
        public async Task<ActionResult<List<FAQToReturnDTO>>> FAQList()
        {
            List<FAQToReturnDTO> ls = await _memberService.FAQList();

            return Ok(ls);
        }
        /// <summary>
        /// دریافت درباره ما
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/AboutUs")]
        [HttpGet("App/AboutUs")]
        [HttpGet("Web/AboutUs")]
        public async Task<ActionResult<AppVariableDTO>> AboutUs()
        {
            AppVariableDTO dto = await _memberService.AboutUs();

            return Ok(dto);
        }

        /// <summary>
        ///  دریافت امنیت و حریم خصوصی 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/SecurityAndPrivacy")]
        [HttpGet("App/SecurityAndPrivacy")]
        [HttpGet("Web/SecurityAndPrivacy")]
        public async Task<ActionResult<AppVariableDTO>> SecurityAndPrivacy()
        {
            AppVariableDTO dto = await _memberService.SecurityAndPrivacy();

            return Ok(dto);
        }

        /// <summary>
        /// دریافت شرایط و ضوابط
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/TermsAndCondition")]
        [HttpGet("App/TermsAndCondition")]
        [HttpGet("Web/TermsAndCondition")]
        public async Task<ActionResult<AppVariableDTO>> TermsAndCondition()
        {
            AppVariableDTO dto = await _memberService.TermsAndCondition();

            return Ok(dto);
        }

        /// <summary>
        /// لیست شهرهای یک استان
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet("Admin/CitiesOfState")]
        [HttpGet("App/CitiesOfState")]
        [HttpGet("Web/CitiesOfState")]
        public async Task<ActionResult<List<CityDTO>>> CitiesOfState(Guid stateId)
        {
            List<CityDTO> ls = await _memberService.GetCitiesOfState(stateId);
            return Ok(ls);
        }

        /// <summary>
        /// لیست استان ها
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/StatesList")]
        [HttpGet("Web/StatesList")]
        [HttpGet("Admin/StatesList")]
        public async Task<ActionResult<List<ProvinceDTO>>> StatesList()
        {
            List<ProvinceDTO> ls = await _memberService.GetStatesList();
            return Ok(ls);
        }
    }
}
