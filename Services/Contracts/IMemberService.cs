using Common.Utilities;
using Entities.DTO;
using Entities.DTO.System;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IMemberService
    {
        Task<LoginResultDTO> Auth(AuthDTO dto);
        Task<OtpResponseDTO> GetOtp(string mobileNumber);
        Task<bool> IsProfileCompleted(Guid userId);
        Task<bool> SetProfile(Guid userId, ProfileToUpdateDTO dto);
        Task<ProfileToReturnDTO> GetProfile(Guid userId);
        Task<List<string>> ProfilePicture(Guid userId, IFormFile img);
        Task<bool> LikeProduct(Guid userId, Guid productId);
        Task<bool> IsProductLikedByMe(Guid userId, Guid productId);
        Task<bool> UnLikeProduct(Guid userId, Guid productId);
        Task<int> LikedProductsCount(Guid userId);
        Task<List<ProductToReturnDTO>> ProductsLikedByMe(Guid userId);
        Task<List<ProvinceDTO>> GetStatesList();
        Task<List<CityDTO>> GetCitiesOfState(Guid stateId);
        Task<List<AddressToReturnDTO>> UserAdressList(Guid userId);
        Task<Guid> PostAddress(Guid userId, AddressForCreateDTO dto);
        Task<bool> DeleteAddress(Guid userId, Guid addressId);
        Task<bool> EditAddress(Guid userId, AddressToReturnDTO dto);
        Task<LoginResultDTO> Login(UserForLoginDTO user);
        Task<List<UserToReturnDTO>> GetUsers();
        Task<UserToReturnDTO> GetUser(Guid userId);
        Task<bool> LikeOrUnlike(Guid userId, Guid productId, bool action);
        Task<List<FAQToReturnDTO>> FAQList();
        Task<AppVariableDTO> AboutUs();
        Task<AppVariableDTO> SecurityAndPrivacy();
        Task<AppVariableDTO> TermsAndCondition();
    }
}