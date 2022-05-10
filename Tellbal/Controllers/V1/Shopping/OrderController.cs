using Common.Utilities;
using Entities.DTO;
using Entities.DTO.Order;
using Entities.Payment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tellbal.Controllers.V1.Shopping
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// افزودن یک آیتم به سبدخرید
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        [HttpGet("App/AddToBasket")]
        [HttpGet("Web/AddToBasket")]
        public async Task<ActionResult<int>> AddToBasket([Required] Guid ItemId, [Required] Guid ColorId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            int result = await _orderService.AddToBasket(userId, ItemId, ColorId);

            return Ok(result);
        }

        /// <summary>
        /// تغییر تعداد محصولات سبد خرید
        /// </summary>
        /// <remarks>
        /// action = false و برای کاهش action = true  برای افزایش 
        /// </remarks>
        /// <param name="action"></param>
        /// <param name="ItemId"></param>
        /// <param name="ColorId"></param>
        /// <returns></returns>
        [HttpGet("App/ChangeProductsCountInBasket")]
        [HttpGet("Web/ChangeProductsCountInBasket")]
        public ActionResult<BasketCountToReturnDTO> ChangeProductsCountInBasket([Required] bool action, [Required] Guid ItemId, [Required] Guid ColorId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            BasketCountToReturnDTO result = _orderService.ChangeProductsCountInBasket(userId, action, ItemId, ColorId);

            return Ok(result);
        }


        /// <summary>
        /// حذف یک آیتم از سبدخرید
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="colorId"></param>
        /// <returns></returns>
        [HttpDelete("App/RemoveFromBasket")]
        [HttpDelete("Web/RemoveFromBasket")]
        public async Task<ActionResult<BasketCountToReturnDTO>> Basket(
            [Required][FromQuery] Guid itemId,
            [Required][FromQuery] Guid colorId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            BasketCountToReturnDTO result = await _orderService.RemoveFromBasket(userId, itemId, colorId);

            return Ok(result);
        }
        /// <summary>
        /// تعداد کالاهای سبدخرید
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/BasketItemCount")]
        [HttpGet("Web/BasketItemCount")]
        public async Task<ActionResult<int>> BasketItemCount()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            int itemCount = await _orderService.GetBasketItemCount(userId);
            return Ok(itemCount);
        }

        /// <summary>
        /// تعیین آدرس سبد خرید
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut("App/BasketAddress")]
        [HttpPut("Web/BasketAddress")]
        public async Task<ActionResult<bool>> SetBasketAddress(BasketAddressDTO order)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool result = await _orderService.SetBasketAddress(userId, order);

            return Ok(result);
        }

        /// <summary>
        /// لیست سفارشات
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/OrdersList")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<OrderToReturnDTO>>> OrdersList(OrderStatusDTO? dto)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            List<OrderToReturnDTO> ls = await _orderService.OrdersList(dto);

            return Ok(ls);
        }

        /// <summary>
        /// تعداد لیست سفارشات بر اساس استاتوس
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/OrderStatusCount")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult<List<OrderStatusCountDTO>> OrderStatusCount()
        {
            List<OrderStatusCountDTO> ls = _orderService.OrderStatusCount();

            return Ok(ls);
        }

        /// <summary>
        /// جزئیات یک سفارش
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Admin/Orders/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<OrderToReturnDTO>> OrderDetail(Guid id)
        {
            OrderToReturnDTO dto = await _orderService.OrderDetail(id);

            return Ok(dto);
        }

        /// <summary>
        /// محصولات موجود در یک سفارش
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/OrderItems/{orderId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<OrderDetailToReturnDTO>>> OrderItems(Guid orderId)
        {
            List<OrderDetailToReturnDTO> ls = await _orderService.OrderItems(orderId);

            return Ok(ls);
        }

        /// <summary>
        /// تغییر وضعیت یک سفارش
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Admin/OrderStatus/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<bool>> ChangeOrderStatus(Guid id, [Required] OrderStatusDTO dto)
        {
            bool res = await _orderService.ChangeOrderStatus(id, dto);

            return Ok(res);
        }

        /// <summary>
        /// سبد خرید من
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/MyBasket")]
        [HttpGet("Web/MyBasket")]
        public async Task<ActionResult<BasketToReturnDTO>> MyBasket()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            BasketToReturnDTO dto = await _orderService.MyBasket(userId);

            return Ok(dto);
        }

        /// <summary>
        /// آیا محصول در سبد خرید من هست؟
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="colorId"></param>
        /// <returns></returns>
        [HttpGet("App/IsProductInMyBasket")]
        [HttpGet("Web/IsProductInMyBasket")]
        public async Task<ActionResult<bool>> IsProductInMyBasket([Required] Guid productId, [Required] Guid colorId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            bool res = await _orderService.IsProductInMyBasket(userId, productId, colorId);

            return Ok(res);
        }

        /// <summary>
        /// لیست سفارشات من
        /// </summary>
        /// <returns></returns>
        [HttpGet("App/MyOrders")]
        [HttpGet("Web/MyOrders")]
        public async Task<ActionResult<List<MyOrderToReturnDTO>>> MyOrders()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            List<MyOrderToReturnDTO> dto = await _orderService.MyOrders(userId);

            return Ok(dto);
        }

        /// <summary>
        /// جزئیات یک سفارش من
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("App/MyOrderDetails/{orderId}")]
        [HttpGet("Web/MyOrderDetails/{orderId}")]
        public async Task<ActionResult<MyOrderToReturnDTO>> MyOrderDetails(Guid orderId)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();


            MyOrderToReturnDTO dto = _orderService.MyOrderDetails(userId, orderId);

            return Ok(dto);
        }

        [HttpGet("App/ApplyCouponToBasket/{basketId}")]
        [HttpGet("Web/ApplyCouponToBasket/{basketId}")]
        public async Task<ActionResult<bool>> ApplyCouponToBasket([FromQuery] string couponCode, [FromRoute] Guid basketId)
        {
            return Ok(Task.FromResult(true));
        }

        [HttpGet("App/RemoveCouponFromBasket/{basketId}")]
        [HttpGet("Web/RemoveCouponFromBasket/{basketId}")]
        public async Task<ActionResult<bool>> RemoveCouponFromBasket([FromRoute] Guid basketId)
        {
            return Ok(Task.FromResult(true));
        }

        /// <summary>
        /// خرید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("App/CheckoutTheBasket")]
        [HttpPost("Web/CheckoutTheBasket")]
        public async Task<ActionResult<string>> CheckoutTheBasket(BasketCheckOutDTO dto)
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();
            var roles = User.GetRoles();

            string result = await _orderService.CheckoutTheBasket(userId, dto);

            return Ok(result);
        }

        /// <summary>
        /// تکمیل خرید توسط کاربر در زرین پال
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet("App/VerifyCheckOut")]
        [HttpGet("Web/VerifyCheckOut")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> VerifyCheckOut([Required][FromQuery] string Authority, [Required][FromQuery] string Status)
        {

            bool result = await _orderService.VerifyCheckOut(Authority, Status);
            string text;
            if (result)
                text = "با موفقیت پرداخت شد";
            else
                text = "پرداخت با مشکل مواجه شد";

            string myres = @$"<!DOCTYPE html>
                <html lang='en'>
                  <head>
                    <meta charset= 'UTF-8''>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' >
                    <title>tellbal payment result</title>
                  <head>
                         <body>
                             <h1>{text}</h1>
                             <a href = 'http://tellbal:screen'> بازگشت به اپ </a>
                          </body>
                </html>";

            return base.Content(myres, "text/html");
        }
    }
}
