using Entities.DTO;
using Entities.DTO.Pricing;
using Entities.DTO.Sell;
using Entities.DTO.System;
using Entities.Product.Customers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IManageService
    {
        Task<bool> DefinePropertyKes(ProductKeysDefinitionsDTO dto);
        Task<bool> AddToPropertyKeys(ProductKeysDefinitionsDTO dto);
        Task<bool> EditPropertyKeys(List<PropertyKeyDTO> list);
        Task<bool> RemovePropertyKeys(Guid id);
        Task<List<PropertyKeyDTO>> GetPropertyKeys(int catId);
        Task<bool> DefineFastPricingKey(FastPricingDefinitionToCreateDTO dto);
        Task<List<FastPricingDefinitionToReturnDTO>> FastPricingList();
        Task<List<FastPricingKeysAndDDsToReturnDTO>> FastPricing(Guid id);
        Task<bool> EditFastPricing(Guid definitionId, List<FastPricingKeysAndDDsToCreateDTO> ls);
        Task<List<SellRequestToReturnDTO>> SellRequestList(SellRequestStatus? status);
        Task<SellRequestToReturnDTO> SellRequest(Guid id);
        Task<bool> ChangeSellRequestStatus(Guid id, SellRequestStatusDTO dto);
        Task<bool> FAQ(FAQToCreateDTO dto);
        Task<List<FAQToReturnDTO>> ArrangeFAQs(List<int> arrangeIds);
        Task<bool> RemoveFAQ(Guid id);
        Task<bool> UpdateAboutUs(AppVariableDTO dto);
        Task<bool> SecurityAndPrivacy(AppVariableDTO dto);
        Task<bool> TermsAndCondition(AppVariableDTO dto);
        Task<FastPricingToReturnDTO> DeviceInSellRequest(Guid reqId);
        bool RemoveFastPricingDefinition(Guid id);
    }
}
