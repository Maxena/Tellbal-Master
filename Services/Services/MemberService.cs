using Common;
using Common.Exceptions;
using Common.Utilities;
using Data;
using Entities.DTO;
using Entities.DTO.System;
using Entities.Identity;
using Entities.User;
using Kavenegar;
using Kavenegar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly SiteSettings _siteSettings;
        private readonly IMemoryCache _otpCache;

        public MemberService(
            IMemoryCache otpCache,
            IOptionsSnapshot<SiteSettings> siteSettings,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _siteSettings = siteSettings.Value;
            _otpCache = otpCache;
        }

        public async Task<LoginResultDTO> Auth(AuthDTO dto)
        {
            var result = new LoginResultDTO
            {
                Message = "خطا در ورود"
            };

            _otpCache.TryGetValue(dto.MobileNumber, out string password);

            if (password != dto.VerificationCode)
            {
                return result;
            }

            var auth = await _signInManager.PasswordSignInAsync(
                dto.MobileNumber,
                dto.VerificationCode,
                false,
                false).ConfigureAwait(false);

            if (auth.Succeeded)
            {
                var userFromDB = await _userManager
                    .FindByNameAsync(dto.MobileNumber).ConfigureAwait(false);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,
                    userFromDB.Id.ToString()),
                    new Claim(ClaimTypes.Name,
                    userFromDB.UserName),
                };

                var roles = await _userManager.GetRolesAsync(userFromDB).ConfigureAwait(false);

                claims.AddRange(
                    roles.ToList()
                    .Select(role =>
                    new Claim(
                        ClaimsIdentity.DefaultRoleClaimType,
                        role)));

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Tokens:Issuer"],
                    _configuration["Tokens:Audience"],
                    claims,
                    expires: DateTime.Now.AddYears(2),
                    signingCredentials: creds);

                result = new LoginResultDTO
                {
                    IsAuthenticated = true,
                    Roles = roles.ToList(),
                    Message = "ورود موفق",
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    FirstName = userFromDB.FirstName,
                    LastName = userFromDB.LastName,
                    UserId = userFromDB.Id,
                    SnapShot = userFromDB.SnapShot,
                };
                // kill password
                await _userManager.RemovePasswordAsync(userFromDB);
            }

            return result;
        }

        public async Task<OtpResponseDTO> GetOtp(string mobileNumber)
        {
            if (mobileNumber.Length != 11)
            {
                return new OtpResponseDTO
                {
                    Success = false,
                    Message = "شماره تلفن باید 11 رقم و با 0 شروع شود.",
                };
            }

            var random = new Random();
            var smsApi = new KavenegarApi(_siteSettings.SmsApiKey);
            var verificationCode = random.Next(1111, 9999).ToString();
            //var result = smsApi.Send("", mobileNumber, $"کد تایید تل بال: {verificationCode}");

            //todo //surround with try catch
            var result = smsApi.VerifyLookup(mobileNumber, verificationCode, "tellbalOtp");

            ////////////////////////////////////////////////////////will remove
            //SendResult result;
            //verificationCode = "1111";
            //result = new SendResult
            //{
            //    Cost = 250,
            //    Date = 22255151,
            //    GregorianDate = DateTime.Now,
            //    Message = "کد : 1111",
            //    Messageid = 1,
            //    Receptor = "09394299889",
            //    Sender = "1000002030",
            //    Status = 1,
            //    StatusText = "ارسال به مخابرات",
            //};
            //////////////////////////////////////////////////////////

            if (result == null) // i think this condition has bug(result will not be null if message not send)
            {
                return new OtpResponseDTO
                {
                    Success = false,
                    Message = "خطا در ارسال پیامک"
                };
            }

            var userFromDb = await _userManager
                .FindByNameAsync(mobileNumber);

            _otpCache.Set(
                mobileNumber,
                verificationCode,
                DateTimeOffset.Now.AddMinutes(2));
            if (userFromDb != null)
            {
                await _userManager.AddPasswordAsync(userFromDb, verificationCode);
            }
            else
            {
                var registerResult = await _userManager.CreateAsync(new User
                {
                    UserName = mobileNumber,
                    PhoneNumber = mobileNumber,
                    FirstName = "",
                    LastName = "",
                    SnapShot = "",/// default pic path
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                }, verificationCode);
            }

            return new OtpResponseDTO
            {
                Success = true,
                Message = "پیامک ارسال شد"
            };
        }

        public async Task<ProfileToReturnDTO> GetProfile(Guid userId)
        {
            var userFromDb = await _userManager.FindByIdAsync(userId.ToString());
            ProfileToReturnDTO dto = new ProfileToReturnDTO
            {
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                City = userFromDb.City,
                Province = userFromDb.Province,
                UserId = userFromDb.Id,
                MobileNumber = userFromDb.PhoneNumber,
                Email = userFromDb.Email,
                Image_L = userFromDb.SnapShot.Replace("_M", "_L"),
                Image_M = userFromDb.SnapShot,
                Image_S = userFromDb.SnapShot.Replace("_M", "_S")
            };
            return dto;
        }

        public async Task<bool> IsProfileCompleted(Guid userId)
        {
            var userFromDb = await _userManager.FindByIdAsync(userId.ToString());
            var result = userFromDb.FirstName != "" ? true : false;
            return result;
        }

        public async Task<bool> LikeProduct(Guid userId, Guid productId)
        {
            if (!await IsProductLikedByMe(userId, productId))
            {
                if (!await _dbContext.Products.AnyAsync(x => x.Id == productId))
                    return false;

                _dbContext.Likes.Add(new Entities.Product.Like
                {
                    UserId = userId,
                    ProductId = productId
                });
                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        public async Task<bool> UnLikeProduct(Guid userId, Guid productId)
        {
            if (await IsProductLikedByMe(userId, productId))
            {
                var existing = await _dbContext.Likes
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

                _dbContext.Likes.Remove(existing);

                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        public async Task<bool> LikeOrUnlike(Guid userId, Guid productId, bool action)
        {
            if (action)
            {
                if (!await IsProductLikedByMe(userId, productId))
                {
                    if (!await _dbContext.Products.AnyAsync(x => x.Id == productId))
                        throw new NotFoundException($"there is no product with : {productId}");

                    _dbContext.Likes.Add(new Entities.Product.Like
                    {
                        UserId = userId,
                        ProductId = productId
                    });
                    if ((await _dbContext.SaveChangesAsync()) > 0)
                        return true;
                }

                throw new BadRequestException("product is already liked");
            }
            else
            {
                if (await IsProductLikedByMe(userId, productId))
                {
                    var existing = await _dbContext.Likes
                        .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

                    _dbContext.Likes.Remove(existing);

                    if ((await _dbContext.SaveChangesAsync()) > 0)
                        return false;
                }

                throw new BadRequestException("product is already unLiked");
            }
        }

        public async Task<List<string>> ProfilePicture(Guid userId, IFormFile img)
        {
            var fileNameBase = userId.ToString();

            var fileNameS = fileNameBase + "_S.jpeg";
            var fileNameM = fileNameBase + "_M.jpeg";
            var fileNameL = fileNameBase + "_L.jpeg";

            var path = @"wwwroot/Photos/";

            var source = img.OpenReadStream();

            ImageHelper.SaveJpeg(source, 200, 200, path + fileNameS, 70);
            ImageHelper.SaveJpeg(source, 600, 600, path + fileNameM, 70);
            ImageHelper.SaveJpeg(source, 1200, 1200, path + fileNameL, 70);

            var userFromDb = await _userManager
                .FindByIdAsync(userId.ToString());

            userFromDb.SnapShot = "/Photos/" + fileNameM;

            var res = await _userManager.UpdateAsync(userFromDb);

            List<string> ls = new();

            if (res.Succeeded)
            {
                ls.Add("/Photos/" + fileNameS);
                ls.Add("/Photos/" + fileNameM);
                ls.Add("/Photos/" + fileNameL);
            }

            return ls;

        }

        public async Task<bool> SetProfile(Guid userId, ProfileToUpdateDTO dto)
        {
            var userFromDb = await _userManager
                .FindByIdAsync(userId.ToString());

            userFromDb.FirstName = dto.FirstName;
            userFromDb.LastName = dto.LastName;
            userFromDb.Province = dto.Province;
            userFromDb.City = dto.City;
            userFromDb.Email = dto.Email;

            var res = await _userManager.UpdateAsync(userFromDb);

            return res.Succeeded;
        }

        public async Task<bool> IsProductLikedByMe(Guid userId, Guid productId)
        {
            return await _dbContext.Likes.AnyAsync(x => x.UserId == userId && x.ProductId == productId);
        }

        public async Task<int> LikedProductsCount(Guid userId)
        {
            return await _dbContext.Likes.CountAsync(x => x.UserId == userId);
        }

        public async Task<List<ProductToReturnDTO>> ProductsLikedByMe(Guid userId)
        {
            var query =
                from l in _dbContext.Likes
                join p in _dbContext.Products
                on l.ProductId equals p.Id
                where l.UserId == userId
                select new ProductToReturnDTO
                {
                    ProductId = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    About = p.About,
                    Discount = p.Discount,
                    Code = p.Code,
                    ProductType = p.ProductType,
                    Images = p.Images.Select(s => new ProductImageToReturnDTO
                    {
                        ImageUrl_L = s.ImageUrl_L,
                        ImageUrl_M = s.ImageUrl_M,
                        ImageUrl_S = s.ImageUrl_S
                    }).ToList()
                };

            return await query.ToListAsync();
        }

        public async Task<List<ProvinceDTO>> GetStatesList()
        {
            return await _dbContext.Provinces
                .Select(x => new ProvinceDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(o => o.Name)
                .ToListAsync();
        }

        public async Task<List<CityDTO>> GetCitiesOfState(Guid stateId)
        {
            return await _dbContext.Cities
                .Where(x => x.ProvinceId == stateId)
                .Select(x => new CityDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(o => o.Name)
                .ToListAsync();
        }

        public async Task<List<AddressToReturnDTO>> UserAdressList(Guid userId)
        {
            return await _dbContext.Addresses
                 .Where(x => x.UserId == userId)
                 .Select(x => new AddressToReturnDTO
                 {
                     AddressId = x.Id,
                     City = x.City,
                     State = x.State,
                     Label = x.Label,
                     DetailedAddress = x.DetailedAddress,
                     ContactName = x.ContactName,
                     ContactNumber = x.ContactNumber,
                     PostalCode = x.PostalCode,
                     UserId = x.UserId
                 }).ToListAsync();
        }

        public async Task<Guid> PostAddress(Guid userId, AddressForCreateDTO dto)
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                City = dto.City,
                State = dto.State,
                ContactNumber = dto.ContactNumber,
                ContactName = dto.ContactName,
                DetailedAddress = dto.DetailedAddress,
                Label = dto.Label,
                PostalCode = dto.PostalCode,
                UserId = userId,
            };

            _dbContext.Addresses.Add(address);

            if (await _dbContext.SaveChangesAsync() <= 0)
                throw new AppException("cant add address");

            return address.Id;
        }

        public async Task<bool> DeleteAddress(Guid userId, Guid addressId)
        {
            if (await _dbContext.Orders.AnyAsync(x => x.AddressId == addressId))
                return false;

            var existing = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == addressId && x.UserId == userId);

            _dbContext.Addresses.Remove(existing);

            return (await _dbContext.SaveChangesAsync() > 0);
        }

        public async Task<bool> EditAddress(Guid userId, AddressToReturnDTO dto)
        {
            if (await _dbContext.Orders.AnyAsync(x => x.AddressId == dto.AddressId))
                throw new PolicyException("address used in application");

            if (dto.UserId != userId)
                return false;

            var existing = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == dto.AddressId && x.UserId == userId);

            existing.City = dto.City;
            existing.State = dto.State;
            existing.ContactNumber = dto.ContactNumber;
            existing.ContactName = dto.ContactName;
            existing.DetailedAddress = dto.DetailedAddress;
            existing.Label = dto.Label;
            existing.PostalCode = dto.PostalCode;

            _dbContext.Addresses.Update(existing);

            return (await _dbContext.SaveChangesAsync() > 0);
        }

        public async Task<LoginResultDTO> Login(UserForLoginDTO user)
        {
            var result = new LoginResultDTO
            {
                Message = "خطا در ورود"
            };

            var auth = await _signInManager.PasswordSignInAsync(
                user.UserName,
                user.PassWord,
                false,
                false).ConfigureAwait(false);

            if (auth.Succeeded)
            {
                var userFromDB = await _userManager.FindByNameAsync(user.UserName).ConfigureAwait(false);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,
                    userFromDB.Id.ToString()),
                    new Claim(ClaimTypes.Name,
                    userFromDB.UserName),
                };

                var roles = await _userManager.GetRolesAsync(userFromDB).ConfigureAwait(false);

                claims.AddRange(
                    roles.ToList()
                    .Select(role =>
                    new Claim(
                        ClaimsIdentity.DefaultRoleClaimType,
                        role)));

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Tokens:Issuer"],
                    _configuration["Tokens:Audience"],
                    claims,
                    expires: DateTime.Now.AddYears(2),
                    signingCredentials: creds);

                result = new LoginResultDTO
                {
                    IsAuthenticated = true,
                    Roles = roles.ToList(),
                    Message = "ورود موفق",
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    FirstName = userFromDB.FirstName,
                    LastName = userFromDB.LastName,
                    UserId = userFromDB.Id,
                    SnapShot = userFromDB.SnapShot,
                };
            }

            return result;
        }

        public async Task<List<UserToReturnDTO>> GetUsers()
        {
            return await _userManager.Users
                 .Where(x => !x.UserRoles.Any())
                 .Select(x => new UserToReturnDTO
                 {
                     Id = x.Id,
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     Email = x.Email,
                     City = x.City,
                     Province = x.Province,
                     MobileNumber = x.PhoneNumber,
                     SnapShot = x.SnapShot,
                     RegisterDate = x.RegisterDate,
                 }).ToListAsync();

        }

        public async Task<UserToReturnDTO> GetUser(Guid userId)
        {
            return await _userManager.Users
                .Where(x => !x.UserRoles.Any() && x.Id == userId)
                .Select(x => new UserToReturnDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    City = x.City,
                    Province = x.Province,
                    Email = x.Email,
                    MobileNumber = x.PhoneNumber,
                    SnapShot = x.SnapShot,
                    RegisterDate = x.RegisterDate,
                }).FirstOrDefaultAsync();
        }

        public async Task<List<FAQToReturnDTO>> FAQList()
        {
            return await _dbContext.FAQs.OrderBy(o => o.Arrange)
                .Select(s => new FAQToReturnDTO
                {
                    Id = s.Id,
                    Answer = s.Answer,
                    Question = s.Question,
                    ArrnageId = s.Arrange,
                }).ToListAsync();
        }

        public async Task<AppVariableDTO> AboutUs()
        {
            return await _dbContext.AppVariables.Where(x => x.Id == 1)
                  .Select(s => new AppVariableDTO
                  {
                      Value = s.AboutUs
                  }).FirstOrDefaultAsync();
        }

        public async Task<AppVariableDTO> SecurityAndPrivacy()
        {
            return await _dbContext.AppVariables.Where(x => x.Id == 1)
                  .Select(s => new AppVariableDTO
                  {
                      Value = s.SecurityAndPrivacy
                  }).FirstOrDefaultAsync();
        }

        public async Task<AppVariableDTO> TermsAndCondition()
        {
            return await _dbContext.AppVariables.Where(x => x.Id == 1)
                  .Select(s => new AppVariableDTO
                  {
                      Value = s.TermsAndConditions
                  }).FirstOrDefaultAsync();
        }
    }
}
