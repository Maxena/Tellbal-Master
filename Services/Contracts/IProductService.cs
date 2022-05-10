using Common.Params;
using Common.Utilities;
using Entities.DTO;
using Entities.DTO.Product;
using Entities.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IProductService
    {
        void GetSubcategories(List<int> list, Category category);
        Task<PagedList<ProductToReturnDTO>> SearchInProducts(PaginationParams<ProductSearch> pagination);
        Task<List<ProductToReturnDTO>> CompareTwoProduct(List<Guid> productIds);
        Task<Guid> AddProduct(ProductForCreateDTO dto);
        Task<bool> EditProduct(Guid productid, ProductForCreateDTO dto);
        Task<List<PriceLogDTO>> ProductPriceLog(Guid productId);
        Task<List<PriceLogDTO>> ProductPriceLogInDateRange(Guid productId, DateTime fromDT, DateTime toDT);
        Task<CreatedImageToReturnDTO> AddImage(IFormFile image);
        Task<bool> RemoveImage(Guid id);
        Task<PagedList<ProductToReturnDTO>> SearchInProductsForAdmin(PaginationParams<ProductSearch> pagination);
        Task<ProductToReturnDTO> GetProducts(Guid id);
        Task<ProductKeysValuesToReturnDTO> GetProductKeysAndValues(Guid id);
        Task<bool> SetProductStatus(Guid productId, Status status);
        ProductToReturnDTO GetProductsForAdmin(Guid id);
    }
}