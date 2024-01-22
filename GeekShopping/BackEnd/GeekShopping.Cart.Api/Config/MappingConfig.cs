using AutoMapper;
using GeekShopping.Cart.Api.Data.ValueObjects;
using GeekShopping.Cart.Api.Model;

namespace GeekShopping.Cart.Api.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps() 
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductVO, Product>().ReverseMap();
            config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
            config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
            config.CreateMap<CartVO, Model.Cart>().ReverseMap();
        });

        return mappingConfig;
    }
}
