using Common.Exceptions;
using Data;
using Entities.DTO;
using Entities.DTO.Pricing;
using Entities.DTO.Sell;
using Entities.DTO.System;
using Entities.Product.Customers;
using Entities.Product.Customers.DynamicPricing;
using Entities.System;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ManageService : IManageService
    {
        private readonly ApplicationDbContext _dbContext;

        public ManageService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddToPropertyKeys(ProductKeysDefinitionsDTO dto)
        {
            return await DefinePropertyKes(dto);
        }

        public async Task AddToFastPricingDefinition(List<FastPricingKeysAndDDsToCreateDTO> dtoList, Guid definitionId)
        {
            foreach (var key in dtoList)
            {
                var keyId = Guid.NewGuid();
                _dbContext.FastPricingKeys.Add(new FastPricingKey
                {
                    Id = keyId,
                    Name = key.Name,
                    Hint = key.Hint,
                    ValueType = key.ValueType,
                    Section = key.Section,
                    FastPricingDefinitionId = definitionId
                });

                await _dbContext.SaveChangesAsync();


                foreach (var dd in key.FastPricingDDs)
                {
                    var opType = OperationType.NoAffectOnPricing;

                    if ((dd.MinRate != null && dd.MaxRate == null) ||
                        (dd.MinRate == null && dd.MaxRate != null))
                    {
                        throw new BadRequestException(BadRequest.PricingRateInCorrectFormat.ToString());
                    }

                    else if ((dd.ErrorTitle != null && dd.ErrorDiscription == null) ||
                       (dd.ErrorTitle == null && dd.ErrorDiscription != null))
                    {
                        throw new BadRequestException(BadRequest.PricingErrorInCorrectFormat.ToString());
                    }

                    else if ((dd.MinRate != null && dd.MaxRate != null && dd.ErrorTitle != null && dd.ErrorDiscription != null))
                    {
                        throw new BadRequestException(BadRequest.PricingBothRateAndErrorInCorrectFormant.ToString());
                    }

                    else if ((dd.MinRate != null && dd.MaxRate != null) &&
                        ((dd.MinRate > 100) ||
                        (dd.MaxRate > 100) ||
                        (dd.MaxRate < 0) ||
                        (dd.MaxRate < 0) ||
                        (dd.MinRate > dd.MaxRate)))
                    {
                        throw new BadRequestException(BadRequest.PricingRateError);
                    }
                    else if (dd.MinRate == null &&
                            dd.MaxRate == null &&
                            dd.ErrorTitle == null &&
                            dd.ErrorDiscription == null)
                    {
                        opType = OperationType.NoAffectOnPricing;
                    }
                    else if (dd.ErrorTitle != null && dd.ErrorDiscription != null)
                        opType = OperationType.ErrorOnPricing;

                    else if (dd.MinRate != null && dd.MaxRate != null)
                        opType = OperationType.PercentPricing;

                    _dbContext.FastPricingDDs.Add(new FastPricingDD
                    {
                        Label = dd.Label,
                        MinRate = dd.MinRate,
                        MaxRate = dd.MaxRate,
                        ErrorTitle = dd.ErrorTitle,
                        ErrorDiscription = dd.ErrorDiscription,
                        OperationType = opType,
                        FastPricingKeyId = keyId,

                    });
                    _dbContext.SaveChanges();
                }
            }
        }

        public async Task<bool> DefineFastPricingKey(FastPricingDefinitionToCreateDTO dto)
        {
            var defId = Guid.NewGuid();

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.FastPricingDefinitions.Add(new FastPricingDefinition
                    {
                        Id = defId,
                        CategoryId = dto.CategoryId,
                        ProductId = dto.ProductId
                    });
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                        {
                            throw new PolicyException("cant define two pricing in one model");
                        }
                    }
                    throw new AppException(ex.Message);
                }

                await AddToFastPricingDefinition(dto.FastPricingKeysAndDDs.ToList(), defId);

                dbContextTransaction.Commit();
                return true;
            }
        }

        public async Task<bool> EditFastPricing(Guid definitionId, List<FastPricingKeysAndDDsToCreateDTO> dtoList)
        {

            var dtoKeyIds = dtoList.Select(s => s.Id).ToList();

            var dbKeys = await _dbContext.FastPricingDefinitions
                .Where(x => x.Id == definitionId)
                .SelectMany(s => s.FastPricingKeys)
                .Include(i => i.FastPricingDDs)
                .ThenInclude(i => i.FastPricingValues)
                .ToListAsync();

            var exsistingKeys = dbKeys
                .Where(x => dtoKeyIds.Contains(x.Id))
                .ToList();

            var removedKeys = dbKeys
                .Where(x => !dtoKeyIds.Contains(x.Id))
                .ToList();

            var addingKeys = dtoList.Where(x => x.Id == null).ToList();

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (removedKeys.Count > 0)
                    {

                        var removedDDs = removedKeys.SelectMany(x => x.FastPricingDDs).ToList();

                        var removedDDIds = removedKeys.SelectMany(x => x.FastPricingDDs).Select(s => s.Id).ToList();

                        var removedValues = await _dbContext.FastPricingValues
                            .Where(x => removedDDIds.Contains(x.Id))
                            .ToListAsync();

                        _dbContext.FastPricingValues.RemoveRange(removedValues);
                        if (await _dbContext.SaveChangesAsync() > 0)
                        {
                            var affectedDevices = _dbContext.Devices
                                .Where(x => x.FastPricingValues
                                    .Any(a => removedValues
                                    .Contains(a)))
                                .ToList();

                            foreach (var item in affectedDevices)
                            {
                                item.IsPriced = false;
                                _dbContext.Devices.Update(item);
                            }
                            _dbContext.SaveChanges();
                        }

                        _dbContext.FastPricingDDs.RemoveRange(removedDDs);
                        await _dbContext.SaveChangesAsync();

                        _dbContext.FastPricingKeys.RemoveRange(removedKeys);
                        if (await _dbContext.SaveChangesAsync() <= 0)
                            throw new AppException("cant remove removed keys");
                    }
                    if (addingKeys.Count > 0)
                    {
                        await AddToFastPricingDefinition(addingKeys, definitionId);
                    }
                    foreach (var item in exsistingKeys)
                    {
                        var newValue = dtoList.Where(s => s.Id == item.Id).FirstOrDefault();

                        item.Name = newValue.Name;
                        item.Section = newValue.Section;
                        item.Hint = newValue.Hint;
                        item.ValueType = newValue.ValueType;

                        _dbContext.FastPricingKeys.Update(item);

                        if (await _dbContext.SaveChangesAsync() <= 0)
                            throw new BadRequestException("cant update existing keys");

                        var existingDDs = item.FastPricingDDs
                            .Where(a => dtoList
                                .Where(x => x.Id == item.Id)
                                .SelectMany(s => s.FastPricingDDs.Select(t => t.Id))
                                .Contains(a.Id))
                            .ToList();

                        var removedDDs = item.FastPricingDDs
                            .Where(a => !dtoList
                                .Where(x => x.Id == item.Id)
                                .SelectMany(s => s.FastPricingDDs.Select(t => t.Id))
                                .Contains(a.Id))
                            .ToList();

                        var addingDDs = dtoList
                            .Where(x => x.Id == item.Id)
                            .SelectMany(s => s.FastPricingDDs.Where(a => a.Id == null))
                            .ToList();

                        foreach (var dd in existingDDs)
                        {
                            var newSubItemValue = dtoList
                                    .Where(x => x.Id == item.Id)
                                    .Select(s => s.FastPricingDDs
                                        .Where(x => x.Id == dd.Id)
                                        .FirstOrDefault())
                                    .FirstOrDefault();


                            var opType = OperationType.NoAffectOnPricing;

                            if ((dd.MinRate != null && dd.MaxRate == null) ||
                                                   (dd.MinRate == null && dd.MaxRate != null))
                            {
                                throw new BadRequestException(BadRequest.PricingRateInCorrectFormat.ToString());
                            }

                            else if ((dd.ErrorTitle != null && dd.ErrorDiscription == null) ||
                               (dd.ErrorTitle == null && dd.ErrorDiscription != null))
                            {
                                throw new BadRequestException(BadRequest.PricingErrorInCorrectFormat.ToString());
                            }

                            else if ((dd.MinRate != null && dd.MaxRate != null && dd.ErrorTitle != null && dd.ErrorDiscription != null))
                            {
                                throw new BadRequestException(BadRequest.PricingBothRateAndErrorInCorrectFormant.ToString());
                            }

                            else if ((dd.MinRate != null && dd.MaxRate != null) &&
                                ((dd.MinRate > 100) ||
                                (dd.MaxRate > 100) ||
                                (dd.MaxRate < 0) ||
                                (dd.MaxRate < 0) ||
                                (dd.MinRate > dd.MaxRate)))
                            {
                                throw new BadRequestException(BadRequest.PricingRateError);
                            }
                            else if (dd.MinRate == null &&
                                    dd.MaxRate == null &&
                                    dd.ErrorTitle == null &&
                                    dd.ErrorDiscription == null)
                            {
                                opType = OperationType.NoAffectOnPricing;
                            }
                            else if (dd.ErrorTitle != null && dd.ErrorDiscription != null)
                                opType = OperationType.ErrorOnPricing;

                            else if (dd.MinRate != null && dd.MaxRate != null)
                                opType = OperationType.PercentPricing;


                            dd.Label = newSubItemValue.Label;
                            dd.MaxRate = newSubItemValue.MaxRate;
                            dd.MinRate = newSubItemValue.MinRate;
                            dd.OperationType = opType;
                            dd.ErrorTitle = newSubItemValue.ErrorTitle;
                            dd.ErrorDiscription = newSubItemValue.ErrorDiscription;

                            _dbContext.FastPricingDDs.Update(dd);

                            if (await _dbContext.SaveChangesAsync() <= 0)
                                throw new BadRequestException("cant update DDs of existing keys");
                        }

                        if (removedDDs.Count > 0)
                        {
                            var removedDDIds = removedDDs.Select(x => x.Id).ToList();

                            var removedValues = await _dbContext.FastPricingValues
                            .Where(x => removedDDIds.Contains(x.Id))
                            .ToListAsync();

                            _dbContext.FastPricingValues.RemoveRange(removedValues);

                            if (await _dbContext.SaveChangesAsync() > 0)
                            {
                                var affectedDevices = _dbContext.Devices
                                    .Where(x => x.FastPricingValues
                                        .Any(a => removedValues
                                        .Contains(a)))
                                    .ToList();

                                foreach (var device in affectedDevices)
                                {
                                    device.IsPriced = false;
                                    _dbContext.Devices.Update(device);
                                }
                                _dbContext.SaveChanges();
                            }

                            _dbContext.FastPricingDDs.RemoveRange(removedDDs);
                            await _dbContext.SaveChangesAsync();
                        }
                        if (addingDDs.Count > 0)
                        {
                            List<FastPricingDD> ddList = new List<FastPricingDD>();
                            foreach (var dd in addingDDs)
                            {
                                var opType = OperationType.NoAffectOnPricing;

                                if ((dd.MinRate != null && dd.MaxRate == null) ||
                                    (dd.MinRate == null && dd.MaxRate != null))
                                {
                                    throw new BadRequestException(BadRequest.PricingRateInCorrectFormat.ToString());
                                }

                                else if ((dd.ErrorTitle != null && dd.ErrorDiscription == null) ||
                                   (dd.ErrorTitle == null && dd.ErrorDiscription != null))
                                {
                                    throw new BadRequestException(BadRequest.PricingErrorInCorrectFormat.ToString());
                                }

                                else if ((dd.MinRate != null && dd.MaxRate != null && dd.ErrorTitle != null && dd.ErrorDiscription != null))
                                {
                                    throw new BadRequestException(BadRequest.PricingBothRateAndErrorInCorrectFormant.ToString());
                                }

                                else if ((dd.MinRate != null && dd.MaxRate != null) &&
                                    ((dd.MinRate > 100) ||
                                    (dd.MaxRate > 100) ||
                                    (dd.MaxRate < 0) ||
                                    (dd.MaxRate < 0) ||
                                    (dd.MinRate > dd.MaxRate)))
                                {
                                    throw new BadRequestException(BadRequest.PricingRateError);
                                }
                                else if (dd.MinRate == null &&
                                        dd.MaxRate == null &&
                                        dd.ErrorTitle == null &&
                                        dd.ErrorDiscription == null)
                                {
                                    opType = OperationType.NoAffectOnPricing;
                                }
                                else if (dd.ErrorTitle != null && dd.ErrorDiscription != null)
                                    opType = OperationType.ErrorOnPricing;

                                else if (dd.MinRate != null && dd.MaxRate != null)
                                    opType = OperationType.PercentPricing;


                                _dbContext.FastPricingDDs.Add(new FastPricingDD
                                {
                                    Label = dd.Label,
                                    MinRate = dd.MinRate,
                                    MaxRate = dd.MaxRate,
                                    ErrorTitle = dd.ErrorTitle,
                                    ErrorDiscription = dd.ErrorDiscription,
                                    OperationType = opType,
                                    FastPricingKeyId = item.Id,

                                });
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw new AppException(ex.Message);
                }
            }
        }

        public bool RemoveFastPricingDefinition(Guid id)
        {
            var dbDefinition = _dbContext.FastPricingDefinitions.Where(x => x.Id == id)
                .FirstOrDefault();


            var dbKeys = _dbContext.FastPricingKeys.Where(x => x.FastPricingDefinitionId == id).ToList();

            var dbDDs = _dbContext.FastPricingDDs
                .Where(x => dbKeys.Select(s => s.Id).ToList().Contains(x.FastPricingKeyId.Value))
                .ToList();

            var dbValues = _dbContext.FastPricingValues
                .Where(x => dbDDs.Select(s => s.Id).ToList().Contains(x.FastPricingDDId))
                .ToList();

            _dbContext.FastPricingValues.RemoveRange(dbValues);
            _dbContext.SaveChanges();

            _dbContext.FastPricingDDs.RemoveRange(dbDDs);
            _dbContext.SaveChanges();

            _dbContext.FastPricingKeys.RemoveRange(dbKeys);
            _dbContext.SaveChanges();

            _dbContext.FastPricingDefinitions.Remove(dbDefinition);
            _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DefinePropertyKes(ProductKeysDefinitionsDTO dto)
        {
            _dbContext.PropertyKeys
                .AddRange(dto.PropertyKeys.Select(x => new Entities.Product.Dynamic.PropertyKey
                {
                    CategoryId = dto.CategoryId,
                    Name = x.Name,
                    KeyType = x.KeyType
                }));

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditPropertyKeys(List<PropertyKeyDTO> list)
        {
            var existings = await _dbContext.PropertyKeys
                 .Where(x => list.Select(s => s.PropertyKeyId).ToList().Contains(x.Id))
                 .ToListAsync();

            foreach (var item in existings)
            {
                var newKey = list.FirstOrDefault(x => x.PropertyKeyId == item.Id);

                item.KeyType = newKey.KeyType;
                item.Name = newKey.Name;

                _dbContext.PropertyKeys.Update(item);
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<FastPricingKeysAndDDsToReturnDTO>> FastPricing(Guid id)
        {
            return await _dbContext.FastPricingDefinitions.Where(x => x.Id == id)
                .Include(i => i.FastPricingKeys)
                .ThenInclude(i => i.FastPricingDDs)
                .Select(s => s.FastPricingKeys
                    .Select(s => new FastPricingKeysAndDDsToReturnDTO
                    {
                        Name = s.Name,
                        Hint = s.Hint,
                        Section = s.Section,
                        FastPricingKeyId = s.Id,
                        ValueType = s.ValueType,
                        FastPricingDDs = s.FastPricingDDs.Select(s => new FastPricingDDsToReturnDTO
                        {
                            Id = s.Id,
                            Label = s.Label,
                            ErrorTitle = s.ErrorTitle,
                            ErrorDiscription = s.ErrorDiscription,
                            OperationType = s.OperationType,
                            MaxRate = s.MaxRate,
                            MinRate = s.MinRate
                        }).ToList()
                    }).ToList()).FirstOrDefaultAsync();
        }

        public async Task<List<FastPricingDefinitionToReturnDTO>> FastPricingList()
        {
            return await _dbContext.FastPricingDefinitions
                 .Include(s => s.Product)
                 .Include(i => i.Category)
                 .ThenInclude(i => i.ParentCategory)
                 .ThenInclude(i => i.ParentCategory)
                 .Select(s => new FastPricingDefinitionToReturnDTO
                 {
                     Category = new CategoryToReturnDTO
                     {
                         ArrangeId = s.Category.ParentCategory!.ParentCategory!.Arrange,
                         CategoryId = s.Category.ParentCategory!.ParentCategory!.Id,
                         ImageUrl_L = s.Category.ParentCategory!.ParentCategory!.ImageUrl_L,
                         ImageUrl_S = s.Category.ParentCategory!.ParentCategory!.ImageUrl_S,
                         ImageUrl_M = s.Category.ParentCategory!.ParentCategory!.ImageUrl_M,
                         Level = s.Category.ParentCategory!.ParentCategory.Level,
                         Name = s.Category.ParentCategory!.ParentCategory!.Name
                     },
                     Brand = new CategoryToReturnDTO
                     {
                         ArrangeId = s.Category.ParentCategory!.Arrange,
                         CategoryId = s.Category.ParentCategory!.Id,
                         ImageUrl_L = s.Category.ParentCategory!.ImageUrl_L,
                         ImageUrl_S = s.Category.ParentCategory!.ImageUrl_S,
                         ImageUrl_M = s.Category.ParentCategory!.ImageUrl_M,
                         Level = s.Category.ParentCategory!.Level,
                         Name = s.Category.ParentCategory!.Name
                     },
                     ProductId = s.ProductId,
                     Id = s.Id,
                     ProductName = s.Product.ProductName
                 }).ToListAsync();
        }

        public async Task<List<PropertyKeyDTO>> GetPropertyKeys(int catId)
        {
            return await _dbContext.PropertyKeys
                .Where(x => x.CategoryId == catId)
                .Select(s => new PropertyKeyDTO
                {
                    PropertyKeyId = s.Id,
                    KeyType = s.KeyType,
                    Name = s.Name
                }).ToListAsync();
        }

        public async Task<bool> RemovePropertyKeys(Guid id)
        {
            var existing = await _dbContext.PropertyKeys
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var keyInValue = await _dbContext.PropertyValues.Where(x => x.PropertyKeyId == existing.Id)
                 .ToListAsync();

            if (keyInValue.Count > 0)
            {
                _dbContext.PropertyValues.RemoveRange(keyInValue);
                await _dbContext.SaveChangesAsync();
            }

            _dbContext.PropertyKeys.Remove(existing);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<FastPricingToReturnDTO> DeviceInSellRequest(Guid reqId)
        {
            var myDevice = await _dbContext.Devices
              .Where(x => x.Id == reqId)
              .Include(x => x.Category)
              .Include(x => x.FastPricingValues)
              .ThenInclude(x => x.FastPricingDD)
              .Include(x => x.FastPricingValues)
              .ThenInclude(x => x.FastPricingKey)
              .ThenInclude(x => x.FastPricingDefinition)
              .ThenInclude(x => x.Product)
              .FirstOrDefaultAsync();


            decimal minimumPrice = 0;
            decimal maximumPrice = 0;
            if (myDevice.IsPriced)
            {
                var DeviceKeys = myDevice.FastPricingValues.Select(x => x.FastPricingDDId).ToList();
                var minRates = myDevice.FastPricingValues.Select(s => s.FastPricingDD.MinRate).ToList();
                var maxRates = myDevice.FastPricingValues.Select(s => s.FastPricingDD.MaxRate).ToList();


                var refPrice = myDevice.FastPricingValues
                    .Select(x => x.FastPricingKey.FastPricingDefinition.Product.Price)
                    .FirstOrDefault();


                maximumPrice = refPrice - (refPrice * ((decimal)minRates.Sum() / 100));
                minimumPrice = refPrice - (refPrice * ((decimal)maxRates.Sum() / 100));
            }

            var dto = new FastPricingToReturnDTO
            {
                DeviceId = myDevice.Id,
                CategoryId = myDevice.CategoryId,
                DT = DateTime.Now,
                MaxPrice = maximumPrice,
                MinPrice = minimumPrice,
                CategoryName = myDevice.Category.Name,
                ImageUrl_L = myDevice.Category.ImageUrl_L,
                ImageUrl_M = myDevice.Category.ImageUrl_M,
                ImageUrl_S = myDevice.Category.ImageUrl_S,
                IsPriced = myDevice.IsPriced,
                FastPricingKeys = myDevice.FastPricingValues
                .Select(s => s.FastPricingKey)
                .Select(x => new FastPricingKeysAndDDsToReturnDTO
                {
                    Name = x.Name,
                    Hint = x.Hint,
                    Section = x.Section,
                    ValueType = x.ValueType,
                    FastPricingKeyId = x.Id,
                    FastPricingDDs = x.FastPricingDDs.Select(a => new FastPricingDDsToReturnDTO
                    {
                        Id = a.Id,
                        Label = a.Label
                    }).ToList()
                }).ToList()
            };

            return dto;
        }

        public async Task<List<SellRequestToReturnDTO>> SellRequestList(SellRequestStatus? status)
        {

            if (status != null)
                return await _dbContext.SellRequests
                    .Include(i => i.Device)
                    .ThenInclude(i => i.Category)
                    .ThenInclude(i => i.ParentCategory)
                    .ThenInclude(i => i.ParentCategory)
                    .Where(x => x.SellRequestStatus == status)
                    .Select(s => new SellRequestToReturnDTO
                    {
                        Id = s.Id,
                        DT = s.DT,
                        SellRequestStatus = s.SellRequestStatus,
                        Code = s.Code,
                        Model = new CategoryToReturnDTO
                        {
                            CategoryId = s.Device.Category.Id,
                            ImageUrl_L = s.Device.Category.ImageUrl_L,
                            ImageUrl_M = s.Device.Category.ImageUrl_M,
                            ImageUrl_S = s.Device.Category.ImageUrl_S,
                            Name = s.Device.Category.Name,
                            ArrangeId = s.Device.Category.Arrange,
                            Level = s.Device.Category.Level
                        },
                        Brand = new CategoryToReturnDTO
                        {
                            CategoryId = s.Device.Category.ParentCategory.Id,
                            ImageUrl_L = s.Device.Category.ParentCategory.ImageUrl_L,
                            ImageUrl_M = s.Device.Category.ParentCategory.ImageUrl_M,
                            ImageUrl_S = s.Device.Category.ParentCategory.ImageUrl_S,
                            Name = s.Device.Category.ParentCategory.Name,
                            ArrangeId = s.Device.Category.ParentCategory.Arrange,
                            Level = s.Device.Category.ParentCategory.Level
                        },
                        Category = new CategoryToReturnDTO
                        {
                            CategoryId = s.Device.Category.ParentCategory.ParentCategory.Id,
                            ImageUrl_L = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_L,
                            ImageUrl_M = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_M,
                            ImageUrl_S = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_S,
                            Name = s.Device.Category.ParentCategory.ParentCategory.Name,
                            ArrangeId = s.Device.Category.ParentCategory.ParentCategory.Arrange,
                            Level = s.Device.Category.ParentCategory.ParentCategory.Level
                        },
                    }).ToListAsync();
            return await _dbContext.SellRequests
                    .Include(i => i.Device)
                    .ThenInclude(i => i.Category)
                    .ThenInclude(i => i.ParentCategory)
                    .ThenInclude(i => i.ParentCategory)
                    .Select(s => new SellRequestToReturnDTO
                    {
                        Id = s.Id,
                        DT = s.DT,
                        SellRequestStatus = s.SellRequestStatus,
                        Code = s.Code,
                        Model = new CategoryToReturnDTO
                        {
                            CategoryId = s.Device.Category.Id,
                            ImageUrl_L = s.Device.Category.ImageUrl_L,
                            ImageUrl_M = s.Device.Category.ImageUrl_M,
                            ImageUrl_S = s.Device.Category.ImageUrl_S,
                            Name = s.Device.Category.Name,
                            ArrangeId = s.Device.Category.Arrange,
                            Level = s.Device.Category.Level
                        },
                        Brand = new CategoryToReturnDTO
                        {
                            CategoryId = s.Device.Category.ParentCategory.Id,
                            ImageUrl_L = s.Device.Category.ParentCategory.ImageUrl_L,
                            ImageUrl_M = s.Device.Category.ParentCategory.ImageUrl_M,
                            ImageUrl_S = s.Device.Category.ParentCategory.ImageUrl_S,
                            Name = s.Device.Category.ParentCategory.Name,
                            ArrangeId = s.Device.Category.ParentCategory.Arrange,
                            Level = s.Device.Category.ParentCategory.Level
                        },
                        Category = new CategoryToReturnDTO
                        {
                            CategoryId = s.Device.Category.ParentCategory.ParentCategory.Id,
                            ImageUrl_L = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_L,
                            ImageUrl_M = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_M,
                            ImageUrl_S = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_S,
                            Name = s.Device.Category.ParentCategory.ParentCategory.Name,
                            ArrangeId = s.Device.Category.ParentCategory.ParentCategory.Arrange,
                            Level = s.Device.Category.ParentCategory.ParentCategory.Level
                        },
                    }).ToListAsync();
        }

        public async Task<SellRequestToReturnDTO> SellRequest(Guid id)
        {
            return await _dbContext.SellRequests.Where(x => x.Id == id)
                .Include(i => i.Address)
                .Include(i => i.Device)
                .ThenInclude(i => i.Category)
                .ThenInclude(i => i.ParentCategory)
                .ThenInclude(i => i.ParentCategory)
                .Select(s => new SellRequestToReturnDTO
                {
                    Id = s.Id,
                    DT = s.DT,
                    AgreedPrice = s.AgreedPrice,
                    Code = s.Code,
                    StatusDescription = s.StatusDescription,
                    SellRequestStatus = s.SellRequestStatus,
                    Address = new AddressToReturnDTO
                    {
                        State = s.Address.State,
                        City = s.Address.City,
                        ContactName = s.Address.ContactName,
                        ContactNumber = s.Address.ContactNumber,
                        DetailedAddress = s.Address.DetailedAddress,
                        PostalCode = s.Address.PostalCode,
                        Label = s.Address.Label,
                        AddressId = s.AddressId,
                        UserId = s.Device.User.Id
                    },
                    Model = new CategoryToReturnDTO
                    {
                        CategoryId = s.Device.Category.Id,
                        ImageUrl_L = s.Device.Category.ImageUrl_L,
                        ImageUrl_M = s.Device.Category.ImageUrl_M,
                        ImageUrl_S = s.Device.Category.ImageUrl_S,
                        Name = s.Device.Category.Name,
                        ArrangeId = s.Device.Category.Arrange,
                        Level = s.Device.Category.Level
                    },
                    Brand = new CategoryToReturnDTO
                    {
                        CategoryId = s.Device.Category.ParentCategory.Id,
                        ImageUrl_L = s.Device.Category.ParentCategory.ImageUrl_L,
                        ImageUrl_M = s.Device.Category.ParentCategory.ImageUrl_M,
                        ImageUrl_S = s.Device.Category.ParentCategory.ImageUrl_S,
                        Name = s.Device.Category.ParentCategory.Name,
                        ArrangeId = s.Device.Category.ParentCategory.Arrange,
                        Level = s.Device.Category.ParentCategory.Level
                    },
                    Category = new CategoryToReturnDTO
                    {
                        CategoryId = s.Device.Category.ParentCategory.ParentCategory.Id,
                        ImageUrl_L = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_L,
                        ImageUrl_M = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_M,
                        ImageUrl_S = s.Device.Category.ParentCategory.ParentCategory.ImageUrl_S,
                        Name = s.Device.Category.ParentCategory.ParentCategory.Name,
                        ArrangeId = s.Device.Category.ParentCategory.ParentCategory.Arrange,
                        Level = s.Device.Category.ParentCategory.ParentCategory.Level
                    },
                    User = new UserToReturnDTO
                    {
                        Id = s.Device.User.Id,
                        FirstName = s.Device.User.FirstName,
                        LastName = s.Device.User.LastName,
                        MobileNumber = s.Device.User.PhoneNumber,
                        Province = s.Device.User.Province,
                        Email = s.Device.User.Email,
                        City = s.Device.User.City,
                        SnapShot = s.Device.User.SnapShot
                    },
                }).FirstOrDefaultAsync();

        }

        public async Task<bool> ChangeSellRequestStatus(Guid id, SellRequestStatusDTO dto)
        {
            var dbSellRequest = await _dbContext.SellRequests.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbSellRequest == null)
                return false;

            dbSellRequest.SellRequestStatus = dto.SellRequestStatus;
            dbSellRequest.StatusDescription = dto.StatusDescription;
            dbSellRequest.AgreedPrice = dto.AgreedPrice;

            _dbContext.SellRequests.Update(dbSellRequest);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FAQ(FAQToCreateDTO dto)
        {

            var latestArrangeId =
            await _dbContext.FAQs
            .OrderByDescending(o => o.Arrange)
            .Select(s => s.Arrange)
            .FirstOrDefaultAsync();

            _dbContext.FAQs.Add(new FAQ
            {
                Answer = dto.Answer,
                Question = dto.Question,
                Arrange = latestArrangeId + 1
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<FAQToReturnDTO>> ArrangeFAQs(List<int> arrangeIds)
        {
            var FAQs = await _dbContext.FAQs
                 .ToListAsync();

            if (FAQs.Count != arrangeIds.Count ||
                FAQs.Select(s => s.Arrange).Any(x => !arrangeIds.Contains(x)))
            {
                throw new BadRequestException("send all arranges");
            }

            foreach (var item in FAQs)
            {
                var index = arrangeIds.IndexOf(item.Arrange);
                item.Arrange = index + 1;
            }

            _dbContext.FAQs.UpdateRange(FAQs);

            await _dbContext.SaveChangesAsync();

            return await _dbContext.FAQs.OrderBy(o => o.Arrange)
                .Select(s => new FAQToReturnDTO
                {
                    Id = s.Id,
                    Answer = s.Answer,
                    Question = s.Question,
                    ArrnageId = s.Arrange,
                }).ToListAsync();
        }

        public async Task<bool> RemoveFAQ(Guid id)
        {
            var dbFAQ = _dbContext.FAQs.Where(x => x.Id == id).FirstOrDefault();

            if (dbFAQ == null)
                throw new NotFoundException($"cant find faq with {id}");

            var removingArrange = dbFAQ.Arrange;

            _dbContext.FAQs.Remove(dbFAQ);

            if (_dbContext.SaveChanges() > 0)
            {
                var upNeedFAQs = await _dbContext.FAQs.Where(x => x.Arrange > removingArrange).ToListAsync();

                foreach (var item in upNeedFAQs)
                {
                    item.Arrange--;
                    _dbContext.FAQs.Update(item);
                }

                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAboutUs(AppVariableDTO dto)
        {
            var dbAbout = _dbContext.AppVariables.Where(x => x.Id == 1).FirstOrDefault();

            dbAbout.AboutUs = dto.Value;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SecurityAndPrivacy(AppVariableDTO dto)
        {
            var dbSecAndPrivacy = _dbContext.AppVariables.Where(x => x.Id == 1).FirstOrDefault();

            dbSecAndPrivacy.SecurityAndPrivacy = dto.Value;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TermsAndCondition(AppVariableDTO dto)
        {
            var dbTermsAndCon = _dbContext.AppVariables.Where(x => x.Id == 1).FirstOrDefault();

            dbTermsAndCon.TermsAndConditions = dto.Value;

            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
