using Entities.DTO;
using Entities.DTO.Order;
using Entities.DTO.Sell;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ISellService
    {
        Task<Guid> AddDevice(Guid userId, FastPricingForCreateDTO dto);
        Task<List<FastPricingKeysAndDDsToReturnDTO>> FastPricingKeysAndValues(int catId);
        Task<FastPricingToReturnDTO> FastPricingValues(FastPricingForCreateDTO dto);
        Task<FastPricingToReturnDTO> MyDevice(Guid userId, Guid id);
        Task<List<FastPricingToReturnDTO>> MyDeviceList(Guid userId);
        Task<bool> SellRequest(SellRequestDTO dto);
        Task<List<SellRequestToReturnDTO>> MySellRequests(Guid userId);
        List<SellRequestStatusCountDTO> SellRequestStatusCount();
    }
}