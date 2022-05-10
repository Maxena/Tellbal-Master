using Common.Exceptions;
using Common.Utilities;
using Data;
using Entities.DTO;
using Entities.DTO.Order;
using Entities.DTO.Product;
using Entities.Payment;
using Entities.User;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPaymentService _paymentService;

        public OrderService(ApplicationDbContext dbContext, IPaymentService paymentService)
        {
            _dbContext = dbContext;
            _paymentService = paymentService;
        }

        public async Task<int> AddToBasket(Guid userId, Guid itemId, Guid ColorId)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == itemId);

            if (product.Status != Entities.Product.Status.Available)
                throw new BadRequestException("product is not available");

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                Guid? BasketId = await GetBasketId(userId);
                if (Guid.Empty == BasketId)
                {
                    await CreateBasket(userId);

                    BasketId = await GetBasketId(userId);
                }

                var Item = await (
                    from o in _dbContext.Orders
                    join od in _dbContext.OrderDetails
                    on o.Id equals od.OrderId
                    join c in _dbContext.Colors
                    on od.ColorId equals c.Id
                    where
                    o.UserId == userId &&
                    o.OrderStatus == OrderStatus.Basket &&
                    od.ProductId == itemId &&
                    od.ColorId == ColorId
                    select od).FirstOrDefaultAsync();

                if (Item is null)
                {
                    _dbContext.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = BasketId.Value,
                        ProductId = itemId,
                        Count = 1,
                        Amount = product.Price,
                        Discount = product.Discount,
                        ColorId = ColorId,
                    });

                    //return (await _dbContext.SaveChangesAsync()) > 0;
                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        var dbOrder = _dbContext.Orders.Where(x => x.Id == BasketId.Value)
                            .FirstOrDefault();

                        dbOrder.Discount += product.Discount;
                        dbOrder.Price += (product.Price - (product.Price * (decimal)product.Discount / 100));
                        dbOrder.TotalPrice += product.Price;

                        _dbContext.Orders.Update(dbOrder);

                        await _dbContext.SaveChangesAsync();

                        dbContextTransaction.Commit();
                        return 1;
                    }
                    dbContextTransaction.Rollback();
                    throw new AppException("failed to create");
                }
                else
                {
                    if (product.ProductType == Entities.Product.ProductType.Used)
                    {
                        throw new PolicyException("cant add many used product");
                    }

                    Item.Count += 1;
                    Item.Amount = product.Price;

                    _dbContext.OrderDetails.Update(Item);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        var dbOrder = _dbContext.Orders.Where(x => x.Id == BasketId.Value)
                            .FirstOrDefault();

                        dbOrder.Discount += product.Discount;
                        dbOrder.Price += (product.Price - (product.Price * (decimal)product.Discount / 100));
                        dbOrder.TotalPrice += product.Price;

                        _dbContext.Orders.Update(dbOrder);

                        await _dbContext.SaveChangesAsync();

                        dbContextTransaction.Commit();
                        return Item.Count;
                    }
                    dbContextTransaction.Rollback();
                    throw new AppException("failed to add");
                }
            }
        }

        public BasketCountToReturnDTO ChangeProductsCountInBasket(Guid userId, bool action, Guid itemId, Guid colorId)
        {

            var dbOrder = _dbContext.Orders.Where(x => x.UserId == userId && x.OrderStatus == OrderStatus.Basket)
                .Include(i => i.OrderDetails)
                .FirstOrDefault();

            if (dbOrder == null)
                throw new NotFoundException("there is no basket");

            var dbOrderDetails = _dbContext.OrderDetails
                .Where(x => x.OrderId == dbOrder.Id && x.ColorId == colorId && x.ProductId == itemId)
                .Include(i => i.Product)
                .FirstOrDefault();


            if (dbOrderDetails == null)
                throw new NotFoundException($"there is no product with given ids in basket");

            if (action == true)
            {
                if (dbOrderDetails.Product.ProductType == Entities.Product.ProductType.Used)
                {
                    throw new PolicyException("cant add many used product");
                }

                dbOrderDetails.Count += 1;

                _dbContext.OrderDetails.Update(dbOrderDetails);

                if (_dbContext.SaveChanges() > 0)
                {
                    dbOrder.Discount += dbOrderDetails.Product.Discount;
                    dbOrder.Price += (dbOrderDetails.Product.Price - (dbOrderDetails.Product.Price * (decimal)dbOrderDetails.Product.Discount / 100));
                    dbOrder.TotalPrice += dbOrderDetails.Product.Price;

                    _dbContext.Orders.Update(dbOrder);

                    _dbContext.SaveChanges();

                    return new BasketCountToReturnDTO
                    {
                        Count = dbOrderDetails.Count,
                        Price = dbOrder.Price,
                        TotalPrice = dbOrder.TotalPrice
                    };
                }
                throw new AppException("cant increase item count");
            }
            else
            {
                if (dbOrderDetails.Count == 1)
                {
                    _dbContext.OrderDetails.Remove(dbOrderDetails);

                    var recordeAffected = _dbContext.SaveChanges();

                    if (recordeAffected > 0)
                    {
                        dbOrder.Discount -= dbOrderDetails.Discount;
                        dbOrder.Price -= (dbOrderDetails.Amount - (dbOrderDetails.Amount * (decimal)dbOrderDetails.Discount / 100));
                        dbOrder.TotalPrice -= dbOrderDetails.Amount;

                        _dbContext.Orders.Update(dbOrder);

                        _dbContext.SaveChanges();

                        return new BasketCountToReturnDTO
                        {
                            Count = 0,
                            Price = dbOrder.Price,
                            TotalPrice = dbOrder.TotalPrice
                        };
                    }
                    throw new AppException("cant decrease item count");
                }
                else
                {
                    dbOrderDetails.Count -= 1;

                    _dbContext.OrderDetails.Update(dbOrderDetails);

                    if (_dbContext.SaveChanges() > 0)
                    {

                        dbOrder.Discount -= dbOrderDetails.Discount;
                        dbOrder.Price -= (dbOrderDetails.Amount - (dbOrderDetails.Amount * (decimal)dbOrderDetails.Discount / 100));
                        dbOrder.TotalPrice -= dbOrderDetails.Amount;

                        _dbContext.Orders.Update(dbOrder);

                        _dbContext.SaveChanges();

                        return new BasketCountToReturnDTO
                        {
                            Count = dbOrderDetails.Count,
                            Price = dbOrder.Price,
                            TotalPrice = dbOrder.TotalPrice
                        };
                    }
                    throw new AppException("cant decrease item count");
                }
            }
        }

        public async Task<BasketCountToReturnDTO> RemoveFromBasket(
        Guid userId,
        Guid itemId,
        Guid colorId)
        {
            var item = await _dbContext.OrderDetails
                .Include(i => i.Order)
                .Where(x =>
                    x.ProductId == itemId
                    &&
                    x.ColorId == colorId
                    &&
                    x.Order.UserId == userId
                    &&
                    x.Order.OrderStatus == OrderStatus.Basket)
                .FirstOrDefaultAsync();

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                if (item != null)
                {
                    _dbContext.OrderDetails.Remove(item);

                    var recordeAffected = await _dbContext.SaveChangesAsync();

                    if (recordeAffected > 0)
                    {
                        var dbOrder = _dbContext.Orders.Where(x => x.Id == item.OrderId)
                            .FirstOrDefault();

                        dbOrder.Discount -= item.Count * item.Discount;
                        dbOrder.Price -= item.Count * (item.Amount - (item.Amount * (decimal)item.Discount / 100));
                        dbOrder.TotalPrice -= item.Count * item.Amount;

                        _dbContext.Orders.Update(dbOrder);

                        _dbContext.SaveChanges();

                        dbContextTransaction.Commit();
                        return new BasketCountToReturnDTO
                        {
                            Count = item.Count,
                            Price = dbOrder.Price,
                            TotalPrice = dbOrder.TotalPrice
                        };
                    }
                }

                dbContextTransaction.Rollback();
                throw new NotFoundException("item not found");
            }
        }

        public async Task<bool> CreateBasket(Guid UserId)
        {
            if (await GetBasketId(UserId) == Guid.Empty)
            {
                var order = new Order
                {
                    UserId = UserId,
                    DT = DateTime.Now,
                    AddressId = null,
                    Code = PID.NewId(),
                    Price = 0,
                    Discount = 0,
                    Tax = 0,
                    TotalPrice = 0,
                    Description = string.Empty,
                    OrderStatus = OrderStatus.Basket,
                    Due = TimeSpan.FromHours(2),
                    CouponId = null
                };

                _dbContext.Orders.Add(order);

                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        public async Task<Guid?> GetBasketId(Guid UserId)
        {
            return await _dbContext.Orders
                .Where(x => x.UserId == UserId && x.OrderStatus == OrderStatus.Basket)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetBasketItemCount(Guid userId)
        {
            return await _dbContext.OrderDetails
                .Where(x => x.Order.OrderStatus == OrderStatus.Basket && x.Order.UserId == userId)
                .SumAsync(s => s.Count);
        }

        public async Task<List<OrderToReturnDTO>> OrdersList(OrderStatusDTO? dto)
        {
            if (dto != null)
                return await _dbContext.Orders
                    .Where(x => x.OrderStatus != OrderStatus.Basket && x.OrderStatus == Enum.Parse<OrderStatus>(dto.ToString()))
                    .Include(i => i.Address)
                    .Include(i => i.OrderDetails)
                    .ThenInclude(i => i.Product)
                    .Include(i => i.User)
                    .Select(s => new OrderToReturnDTO
                    {
                        DT = s.DT,
                        OrderId = s.Id,
                        Address = new AddressToReturnDTO
                        {
                            City = s.Address.City,
                            State = s.Address.State,
                        },
                        OrderStatus = Enum.Parse<OrderStatusDTO>(s.OrderStatus.ToString()),
                        Description = s.Description,
                        User = new UserToReturnDTO
                        {
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            MobileNumber = s.User.PhoneNumber,
                        },
                        Price = s.Price,
                        Code = s.Code,
                        TotalPrice = s.TotalPrice,
                    }).ToListAsync();

            return await _dbContext.Orders
                    .Where(x => x.OrderStatus != OrderStatus.Basket)
                    .Include(i => i.Address)
                    .Include(i => i.OrderDetails)
                    .ThenInclude(i => i.Product)
                    .Include(i => i.User)
                    .Select(s => new OrderToReturnDTO
                    {
                        DT = s.DT,
                        OrderId = s.Id,
                        Address = new AddressToReturnDTO
                        {
                            City = s.Address.City,
                            State = s.Address.State,
                        },
                        OrderStatus = Enum.Parse<OrderStatusDTO>(s.OrderStatus.ToString()),
                        Description = s.Description,
                        User = new UserToReturnDTO
                        {
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            MobileNumber = s.User.PhoneNumber,
                        },
                        Price = s.Price,
                        Code = s.Code,
                        TotalPrice = s.TotalPrice,
                    }).ToListAsync();
        }

        public async Task<OrderToReturnDTO> OrderDetail(Guid id)
        {
            var result = await _dbContext.Orders
               .Where(x => x.OrderStatus != OrderStatus.Basket && x.Id == id)
               .Include(i => i.User)
               .Include(i => i.Address)
               .Include(i => i.OrderDetails)
               .ThenInclude(i => i.Product).ToListAsync();
            return result
               .Select(s => new OrderToReturnDTO
               {
                   DT = s.DT,
                   OrderId = s.Id,
                   Code = s.Code,
                   Discount = s.Discount,
                   Tax = s.Tax,
                   Address = new AddressToReturnDTO
                   {
                       City = s.Address.City,
                       State = s.Address.State,
                       DetailedAddress = s.Address.DetailedAddress,
                       ContactName = s.Address.ContactName,
                       ContactNumber = s.Address.ContactNumber,
                       Label = s.Address.Label,
                       AddressId = s.Address.Id,
                       PostalCode = s.Address.PostalCode,
                       UserId = s.Address.UserId
                   },
                   OrderStatus = Enum.Parse<OrderStatusDTO>(s.OrderStatus.ToString()),
                   Description = s.Description,
                   User = new UserToReturnDTO
                   {
                       FirstName = s.User.FirstName,
                       LastName = s.User.LastName,
                       MobileNumber = s.User.PhoneNumber,
                       Province = s.User.Province,
                       City = s.User.City,
                       Email = s.User.Email,
                       RegisterDate = s.User.RegisterDate,
                       SnapShot = s.User.SnapShot,
                       Id = s.User.Id
                   },
                   Price = s.Price,
                   TotalPrice = s.TotalPrice
               }).FirstOrDefault();
        }

        public async Task<BasketToReturnDTO> MyBasket(Guid userId)
        {
            var result = await _dbContext.Orders
                .Where(x => x.UserId == userId && x.OrderStatus == OrderStatus.Basket)
                .Include(i => i.OrderDetails)
                .ThenInclude(i => i.Color)
                .ThenInclude(i => i.Product)
                .Select(s => new BasketToReturnDTO
                {
                    BasketId = s.Id,
                    Price = s.Price,
                    TotalPrice = s.TotalPrice,
                    Tax = s.Tax,
                    BasketDetails = s.OrderDetails.Select(a => new BasketDetailToReturnDTO
                    {
                        ProductId = a.Product.Id,
                        ProductName = a.Product.ProductName,
                        About = a.Product.About,
                        Description = a.Product.Description,
                        Code = a.Product.Code,
                        Count = a.Count,
                        Price = a.Product.Price,
                        Status = a.Product.Status,
                        Discount = a.Product.Discount,
                        CategoryId = a.Product.CategoryId,
                        ProductType = a.Product.ProductType,
                        Images = a.Product.Images.Select(a => new ProductImageToReturnDTO
                        {
                            Id = a.Id,
                            ImageUrl_L = a.ImageUrl_L,
                            ImageUrl_M = a.ImageUrl_M,
                            ImageUrl_S = a.ImageUrl_S
                        }).ToList(),
                        Color = new ColorDTO
                        {
                            Code = a.Color.Code,
                            Id = a.Color.Id,
                            Name = a.Color.Name,
                        }
                    }).ToList()

                }).FirstOrDefaultAsync();

            // generate 204 code for empty basket
            if (result is not null && result.BasketDetails.Count <= 0)
                return null;

            return result;

            //var basketId = await GetBasketId(userId);

            //if (basketId == Guid.Empty)
            //{
            //    return null;
            //}

            //var dbOrder = _dbContext.Orders.Where(x => x.Id == basketId).FirstOrDefault();

            //var basketProductIds = _dbContext.OrderDetails.Where(x => x.OrderId == basketId)
            //    .Select(x => x.ProductId)
            //    .ToList();

            //if (basketProductIds.Count <= 0)
            //    return null;

            //var basketColorIds = _dbContext.OrderDetails.Where(x => x.OrderId == basketId)
            //    .Select(x => x.ColorId)
            //    .ToList();

            //var basketProducts = await _dbContext.Products
            //    .Where(x => basketProductIds.Contains(x.Id))
            //    .Include(i => i.Images)
            //    .Include(i => i.Colors.Where(x => basketColorIds.Contains(x.Id)))
            //    .ToListAsync();

            //List<ProductToReturnDTO> ls = new();
            //foreach (var item in basketProducts)
            //{
            //    ls.Add(new ProductToReturnDTO
            //    {
            //        ProductName = item.ProductName,
            //        ProductId = item.Id,
            //        About = item.About,
            //        Code = item.Code,
            //        Status = item.Status,
            //        Description = item.Description,
            //        Price = item.Price,
            //        Discount = item.Discount,
            //        CategoryId = item.CategoryId,
            //        ProductTestDescription = item.ProductTestDescription,
            //        ProductType = item.ProductType,
            //        Warranty = item.Warranty,
            //        Colors = item.Colors.Select(s => new ColorDTO
            //        {
            //            Id = s.Id,
            //            Name = s.Name,
            //            Code = s.Code,
            //        }).ToList(),
            //        Images = item.Images.Select(s => new ProductImageToReturnDTO
            //        {
            //            Id = s.Id,
            //            ImageUrl_L = s.ImageUrl_L,
            //            ImageUrl_M = s.ImageUrl_M,
            //            ImageUrl_S = s.ImageUrl_S
            //        }).ToList(),

            //    });
            //}
            //var dto = new BasketToReturnDTO
            //{
            //    BasketId = basketId.Value,
            //    Price = dbOrder.Price,
            //    TotalPrice = dbOrder.TotalPrice,
            //    Products = ls,
            //    Tax = dbOrder.Tax,
            //};

            //return dto;

        }

        public async Task<bool> ChangeOrderStatus(Guid id, OrderStatusDTO dto)
        {
            var dbOrder = await _dbContext.Orders.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbOrder == null)
                return false;

            dbOrder.OrderStatus = Enum.Parse<OrderStatus>(dto.ToString());

            _dbContext.Orders.Update(dbOrder);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<OrderDetailToReturnDTO>> OrderItems(Guid orderId)
        {
            return await _dbContext.OrderDetails.Where(x => x.OrderId == orderId)
                .Include(i => i.Product)
                .Include(i => i.Color)
                .Select(s => new OrderDetailToReturnDTO
                {
                    Amount = s.Amount,
                    Color = new ColorDTO
                    {
                        Id = s.Color.Id,
                        Code = s.Color.Code,
                        Name = s.Color.Name
                    },
                    ColorId = s.ColorId,
                    Count = s.Count,
                    Discount = s.Discount,
                    ProductId = s.ProductId,
                    Product = new ProductToReturnDTO
                    {
                        ProductName = s.Product.ProductName,
                        ProductId = s.Product.Id,
                        Price = s.Product.Price,
                        Discount = s.Product.Discount,
                        About = s.Product.About,
                        Status = s.Product.Status,
                        CategoryId = s.Product.CategoryId,
                        Description = s.Product.Description,
                        ProductTestDescription = s.Product.ProductTestDescription,
                        Warranty = s.Product.Warranty,
                        Code = s.Product.Code,
                    },
                }).ToListAsync();
        }

        public async Task<List<MyOrderToReturnDTO>> MyOrders(Guid userId)
        {
            return await _dbContext.Orders
                .Where(x => x.UserId == userId && x.OrderStatus != OrderStatus.Basket)
                .Include(i => i.OrderDetails)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.Images)
                .Select(s => new MyOrderToReturnDTO
                {
                    OrderId = s.Id,
                    Code = s.Code,
                    DT = s.DT,
                    OrderStatus = Enum.Parse<OrderStatusDTO>(s.OrderStatus.ToString()),
                    Count = s.OrderDetails.Select(a => a.Count).Sum(),
                    Price = s.Price,

                    ProductsInOrder = s.OrderDetails.Select(a => new ProductInOrderToReturnDTO
                    {
                        ProductId = a.Product.Id,
                        ProductName = a.Product.ProductName,
                        Count = a.Count,
                        Images = a.Product.Images.Select(b => new ProductImageToReturnDTO
                        {
                            Id = b.Id,
                            ImageUrl_L = b.ImageUrl_L,
                            ImageUrl_M = b.ImageUrl_M,
                            ImageUrl_S = b.ImageUrl_S
                        }).Take(1).ToList()
                    }).ToList()
                }).ToListAsync();


            //var dbOrder = await _dbContext.Orders
            //    .Where(x => x.UserId == userId && x.OrderStatus != OrderStatus.Basket)
            //    .FirstOrDefaultAsync();

            //if (dbOrder == null)
            //    return null;

            //return await _dbContext.OrderDetails
            //    .Where(x => x.OrderId == dbOrder.Id)
            //    .Include(i => i.Product)
            //    .ThenInclude(i => i.Images)
            //    .Select(s => new MyOrderToReturnDTO
            //    {
            //        OrderId = s.Order.Id,
            //        Code = s.Order.Code,
            //        DT = s.Order.DT,
            //        OrderStatus = Enum.Parse<OrderStatusDTO>(s.Order.OrderStatus.ToString()),
            //        Count = s.Count
            //        Price = s.Order.Price,
            //        ProductsInOrder =
            //    }).ToListAsync();
        }

        public MyOrderToReturnDTO MyOrderDetails(Guid userId, Guid orderId)
        {
            return _dbContext.Orders
                .Where(x => x.UserId == userId && x.OrderStatus != OrderStatus.Basket && x.Id == orderId)
                .Include(i => i.OrderDetails)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.Images)
                .Include(i => i.Address)
                .Select(s => new MyOrderToReturnDTO
                {
                    OrderId = s.Id,
                    Code = s.Code,
                    DT = s.DT,
                    OrderStatus = Enum.Parse<OrderStatusDTO>(s.OrderStatus.ToString()),
                    Count = s.OrderDetails.Select(a => a.Count).Sum(),
                    Price = s.Price,
                    TotalPrice = s.TotalPrice,
                    Tax = s.Tax,
                    Address = new AddressToReturnDTO
                    {
                        AddressId = s.Address.Id,
                        City = s.Address.City,
                        State = s.Address.State,
                        ContactName = s.Address.ContactName,
                        ContactNumber = s.Address.ContactNumber,
                        DetailedAddress = s.Address.DetailedAddress,
                        Label = s.Address.Label,
                        PostalCode = s.Address.PostalCode,
                    },
                    ProductsInOrder = s.OrderDetails.Select(a => new ProductInOrderToReturnDTO
                    {
                        ProductId = a.Product.Id,
                        ProductName = a.Product.ProductName,
                        About = a.Product.About,
                        Code = a.Product.Code,
                        Status = a.Product.Status,
                        ProductType = a.Product.ProductType,
                        Discount = a.Product.Discount,
                        CategoryId = a.Product.CategoryId,
                        Count = a.Count,
                        Description = a.Product.Description,
                        Price = a.Product.Price,
                        Color = new ColorDTO
                        {
                            Code = a.Color.Code,
                            Id = a.Color.Id,
                            Name = a.Color.Name
                        },
                        Images = a.Product.Images.Select(b => new ProductImageToReturnDTO
                        {
                            Id = b.Id,
                            ImageUrl_L = b.ImageUrl_L,
                            ImageUrl_M = b.ImageUrl_M,
                            ImageUrl_S = b.ImageUrl_S
                        }).Take(1).ToList()
                    }).ToList()
                }).FirstOrDefault();
        }

        public async Task<bool> IsProductInMyBasket(Guid userId, Guid productId, Guid colorId)
        {
            return await _dbContext.Orders
                .Include(i => i.OrderDetails)
                .Where(x => x.OrderStatus == OrderStatus.Basket && x.UserId == userId)
                .AnyAsync(s => s.OrderDetails.Any(a => a.ProductId == productId && a.ColorId == colorId));
        }

        public async Task<bool> SetBasketAddress(Guid userId, BasketAddressDTO dto)
        {
            var dbOrder = await _dbContext.Orders
                .Where(x => x.Id == dto.BasketId && x.OrderStatus == OrderStatus.Basket && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (dbOrder == null)
                throw new NotFoundException("there in no basket with given id");

            dbOrder.AddressId = dto.AddressId;

            _dbContext.Orders.Update(dbOrder);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public List<OrderStatusCountDTO> OrderStatusCount()
        {
            var dbOrder = _dbContext.Orders
                .Where(x => x.OrderStatus != OrderStatus.Basket)
                .AsEnumerable()
                .GroupBy(g => g.OrderStatus)
                .Select(s => new OrderStatusCountDTO
                {
                    OrderStatus = Enum.Parse<OrderStatusDTO>(s.Select(a => a.OrderStatus).FirstOrDefault().ToString()),
                    Count = s.Count()
                }).ToList();

            return dbOrder;
        }

        public async Task<string> CheckoutTheBasket(Guid userId, BasketCheckOutDTO dto)
        {
            var dbBasket = _dbContext.Orders
                 .Where(x => x.Id == dto.BasketId && x.OrderStatus == OrderStatus.Basket)
                 .FirstOrDefault();

            if (dbBasket.AddressId == null)
                throw new BadRequestException("basket must contains address");


            RequestForPayResponse response = await _paymentService.RequestForPay(dto.PaymentGateWayId, dbBasket);

            if (response.PaymentResponse.Status == 100)
            {
                _dbContext.Payments.Add(new Payment
                {
                    OrderId = dbBasket.Id,
                    DT = DateTime.Now,
                    Authority = response.PaymentResponse.Authority,
                    IsSuccess = false,
                    MerchantID = response.MerchantId,
                    RefID = 0,
                    Status = 0,
                    CardHash = null,
                    CardPan = null,
                    Fee = 0,
                    FeeType = null
                });

                _dbContext.SaveChanges();

                return response.PaymentResponse.PaymentURL;
            }
            throw new AppException("مشکل در اتصال به درگاه پرداخت");

        }

        public async Task<bool> VerifyCheckOut(string authority, string status)
        {

            var dbPayment = _dbContext.Payments.Where(x => x.Authority == authority)
                .Include(i => i.Order)
                .FirstOrDefault();

            if (dbPayment == null)
                throw new AppException(Common.ApiResultStatusCode.UnAuthorized, "unAuthorized Request", HttpStatusCode.Unauthorized);

            VerificationResponse response = await _paymentService.VerifyPayment(dbPayment);

            if (response.IsSuccess == true && response.Status == 100 || response.Status == 101)
            {
                dbPayment.IsSuccess = true;
                dbPayment.RefID = response.RefID;
                dbPayment.Status = response.Status;
                dbPayment.DT = DateTime.Now;

                _dbContext.Payments.Update(dbPayment);

                _dbContext.SaveChanges();

                var dbOrder = _dbContext.Orders.Where(x => x.Id == dbPayment.OrderId).FirstOrDefault();

                dbOrder.OrderStatus = OrderStatus.Pending;

                _dbContext.Orders.Update(dbOrder);

                _dbContext.SaveChanges();


                return true;
            }
            return false;
        }
    }
}
