using AutoMapper;
using GeekShopping.Product.Api.Data.ValueObjects;

namespace GeekShopping.Product.Api.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductVO, Model.Product>().ReverseMap();
        });

        return mappingConfig;
    }
}
