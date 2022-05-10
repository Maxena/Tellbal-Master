using Entities.DTO;
using Entities.DTO.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<int> GetBasketItemCount(Guid userId);
        Task<BasketCountToReturnDTO> RemoveFromBasket(Guid userId, Guid itemId, Guid colorId);
        Task<int> AddToBasket(Guid userId, Guid itemId, Guid ColorId);
        Task<Guid?> GetBasketId(Guid UserId);
        Task<bool> CreateBasket(Guid UserId);
        Task<List<OrderToReturnDTO>> OrdersList(OrderStatusDTO? dto);
        Task<BasketToReturnDTO> MyBasket(Guid userId);
        Task<OrderToReturnDTO> OrderDetail(Guid id);
        Task<bool> ChangeOrderStatus(Guid id, OrderStatusDTO dto);
        Task<List<OrderDetailToReturnDTO>> OrderItems(Guid orderId);
        Task<List<MyOrderToReturnDTO>> MyOrders(Guid userId);
        Task<bool> IsProductInMyBasket(Guid userId, Guid productId, Guid colorId);
        Task<bool> SetBasketAddress(Guid userId, BasketAddressDTO dto);
        BasketCountToReturnDTO ChangeProductsCountInBasket(Guid userId, bool action, Guid itemId, Guid colorId);
        List<OrderStatusCountDTO> OrderStatusCount();
        MyOrderToReturnDTO MyOrderDetails(Guid userId, Guid orderId);
        Task<string> CheckoutTheBasket(Guid userId, BasketCheckOutDTO dto);
        Task<bool> VerifyCheckOut(string authority, string status);
    }
}
