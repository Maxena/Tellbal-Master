using Common.Utilities;
using Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tellbal.Controllers.V1.Management
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,SuperAdmin")]
    public class ManageProfileController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public ManageProfileController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        /// <summary>
        /// ورود
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Admin/Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResultDTO>> LogIn([FromBody] UserForLoginDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LoginResultDTO result = await _memberService.Login(user);

            return Ok(result);
        }

        /// <summary>
        /// ویرایش تصویر پروفایل
        /// </summary>
        /// <remarks>
        /// آدرس تصویر آپلود شده
        /// </remarks>
        /// <param name="Img"></param>
        /// <returns></returns>
        [HttpPut("Admin/ProfilePicture")]
        public async Task<ActionResult<List<string>>> ProfilePicture(IFormFile Img)
        {
            var userId = User.GetUserId();

            List<string> result = await _memberService.ProfilePicture(userId, Img);

            return Ok(result);
        }

        /// <summary>
        /// دریافت پروفایل
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/Profile")]
        public async Task<ActionResult<ProfileToReturnDTO>> Profile()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            ProfileToReturnDTO dto = await _memberService.GetProfile(userId);
            return Ok(dto);
        }

        /// <summary>
        /// تکمیل پروفایل
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Admin/Profile")]
        public async Task<ActionResult<bool>> Profile([FromBody] ProfileToUpdateDTO dto)
        {
            var userId = User.GetUserId();

            bool result = await _memberService.SetProfile(userId, dto);

            return Ok(result);
        }

        [HttpPost("Admin/RefreshToken")]
        public ActionResult<LoginResultDTO> RefreshToken()
        {
            return Ok(new LoginResultDTO());
        }

        [HttpPost("Admin/ChangePassword")]
        public ActionResult<LoginResultDTO> ChangePassword()
        {
            return Ok(new LoginResultDTO());
        }
    }
}
