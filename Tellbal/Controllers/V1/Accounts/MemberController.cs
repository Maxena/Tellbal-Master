using Common.Utilities;
using Entities.DTO;
using Entities.DTO.System;
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

    public class MemberController : ControllerBase
    {
        #region Configurations
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #endregion

        #region APIs
        /// <summary>
        /// دریافت کد ورود با پیامک
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        [HttpGet("App/GetOtp")]
        [HttpGet("Web/GetOtp")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OtpResponseDTO>> GetOtp([FromQuery][Required] string mobileNumber)
        {
            OtpResponseDTO result = await _memberService.GetOtp(mobileNumber);

            return Ok(result);
        }

        /// <summary>
        /// ورود با شماره تلفن و کد پیامکی
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>نتیجه ورود</returns>
        /// <response code="200">return user associated with mobile number</response>
        /// <response code="400">if verfication code invalid</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("App/Auth")]
        [HttpPost("Web/Auth")]
        public async Task<ActionResult<LoginResultDTO>> Auth(AuthDTO dto)
        {
            LoginResultDTO lr = await _memberService.Auth(dto);

            return Ok(lr);
        }



        /// <summary>
        /// آیا پروفایل تکمیل شده است؟
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/IsProfileCompleted")]
        [HttpGet("Web/IsProfileCompleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> IsProfileCompleted()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool result = await _memberService.IsProfileCompleted(userId);
            return Ok(result);
        }

        /// <summary>
        /// تکمیل پروفایل
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("App/Profile")]
        [HttpPut("Web/Profile")]
        public async Task<ActionResult<bool>> Profile([FromBody] ProfileToUpdateDTO dto)
        {
            var userId = User.GetUserId();

            bool result = await _memberService.SetProfile(userId, dto);

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
        [HttpPut("App/ProfilePicture")]
        [HttpPut("Web/ProfilePicture")]
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
        [HttpGet("App/Profile")]
        [HttpGet("Web/Profile")]
        public async Task<ActionResult<ProfileToReturnDTO>> Profile()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            ProfileToReturnDTO dto = await _memberService.GetProfile(userId);
            return Ok(dto);
        }

        /// <summary>
        /// لایک کردن یا آنلایک کردن محصول
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpGet("App/LikeProduct/{productId}")]
        [HttpGet("Web/LikeProduct/{productId}")]
        public async Task<ActionResult<bool>> LikeOrUnlike(Guid productId, [Required] bool action)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool response = await _memberService.LikeOrUnlike(userId, productId, action);

            return Ok(response);
        }
        /// <summary>
        /// لایک کردن محصول
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("App/LikeProduct")]
        [HttpGet("Web/LikeProduct")]
        public async Task<ActionResult<bool>> LikeProduct(Guid productId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool response = await _memberService.LikeProduct(userId, productId);

            return Ok(response);
        }

        /// <summary>
        /// آنلایک کردن محصول
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("App/UnLikeProduct")]
        [HttpGet("Web/UnLikeProduct")]
        public async Task<ActionResult<bool>> UnLikeProduct(Guid productId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool response = await _memberService.UnLikeProduct(userId, productId);

            return Ok(response);
        }

        /// <summary>
        /// آیا محصول توسط من لایک شده است؟
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("App/IsProductLikedByMe")]
        [HttpGet("Web/IsProductLikedByMe")]
        public async Task<ActionResult<bool>> IsProductLikedByMe(Guid productId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool response = await _memberService.IsProductLikedByMe(userId, productId);

            return Ok(response);
        }

        /// <summary>
        /// تعداد کالاهایی که من لایک کرده ام
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/LikedProductsCount")]
        [HttpGet("Web/LikedProductsCount")]
        public async Task<ActionResult<int>> LikedProductsCount()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            int res = await _memberService.LikedProductsCount(userId);

            return Ok(res);
        }

        /// <summary>
        /// لیست کالاهایی که من لایک کرده ام
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/ProductsLikedByMe")]
        [HttpGet("Web/ProductsLikedByMe")]
        public async Task<ActionResult<List<ProductToReturnDTO>>> ProductsLikedByMe()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            List<ProductToReturnDTO> ls = await _memberService.ProductsLikedByMe(userId);

            return Ok(ls);
        }

        /// <summary>
        /// لیست آدرس های من
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/UserAdressList")]
        [HttpGet("Web/UserAdressList")]
        public async Task<ActionResult<List<AddressToReturnDTO>>> UserAdressList()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            List<AddressToReturnDTO> ls = await _memberService.UserAdressList(userId);
            return Ok(ls);
        }

        /// <summary>
        /// لیست آدرس های یک کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("Admin/UserAdressList")]
        public async Task<ActionResult<List<AddressToReturnDTO>>> GetUserAdressList(Guid userId)
        {
            List<AddressToReturnDTO> ls = await _memberService.UserAdressList(userId);
            return Ok(ls);
        }
        /// <summary>
        /// ثبت آدرس
        /// </summary>
        /// <returns></returns>
        [HttpPost("App/Address")]
        [HttpPost("Web/Address")]
        public async Task<ActionResult<Guid>> Address(AddressForCreateDTO dto)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            Guid res = await _memberService.PostAddress(userId, dto);

            return Ok(res);
        }

        /// <summary>
        /// حذف آدرس من
        /// </summary>
        /// <remarks>
        /// اگر در هیچ سفارشی استفاده نشده باشد میتوان حذف کرد
        /// </remarks>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpDelete("App/Address")]
        [HttpDelete("Web/Address")]
        public async Task<ActionResult<bool>> Address(Guid addressId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool res = await _memberService.DeleteAddress(userId, addressId);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش آدرس من
        /// </summary>
        /// <remarks>
        /// اگر در هیچ سفارشی استفاده نشده باشد میتوان ویرایش کرد
        /// </remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("App/Address")]
        [HttpPut("Web/Address")]
        public async Task<ActionResult<bool>> Address(AddressToReturnDTO dto)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool res = await _memberService.EditAddress(userId, dto);

            return Ok(res);
        }

        /// <summary>
        /// لیست کاربر ها
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/UserList")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<UserToReturnDTO>>> GetUsers()
        {
            List<UserToReturnDTO> ls = await _memberService.GetUsers();

            return Ok(ls);
        }
        /// <summary>
        /// دریافت اطلاعات یک کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/User/{userId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<UserToReturnDTO>> GetUser(Guid userId)
        {
            UserToReturnDTO dto = await _memberService.GetUser(userId);

            return Ok(dto);
        }

        //[HttpGet("App/UnreadNotificationsCount")]
        //public async Task<ActionResult<bool>> UnreadNotificationsCount()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/NotificationsList")]
        //public async Task<ActionResult<bool>> NotificationsList()
        //{
        //    return Ok(Task.FromResult(true));
        //}

        //[HttpGet("App/SetSeenNotification")]
        //public async Task<ActionResult<bool>> SetSeenNotification()
        //{
        //    return Ok(Task.FromResult(true));
        //}
        #endregion

    }
}
