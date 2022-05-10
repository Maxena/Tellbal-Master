using AutoMapper;
using Entities.Product;
using Entities.Product.Customers;
using Entities.Product.Dynamic;

namespace Tellbal
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            //CreateMap<Device, CustomerProductDto>().ReverseMap();
            //CreateMap<PropertyKey, PropertyKeyDto>().ReverseMap();
            //CreateMap<CategoryDto, Category>().ReverseMap();
            //.ForMember(p => p.Author, opt => opt.Ignore())
            //.ForMember(p => p.Category, opt => opt.Ignore());
        }
    }
}
