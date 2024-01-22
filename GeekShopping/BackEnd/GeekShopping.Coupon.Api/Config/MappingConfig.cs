using AutoMapper;
using GeekShopping.Coupon.Api.Data.ValueObjects;

namespace GeekShopping.Coupon.Api.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps() 
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CouponVO, Model.Coupon>().ReverseMap();
        });

        return mappingConfig;
    }
}
