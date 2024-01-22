using AutoMapper;
using GeekShopping.Coupon.Api.Data.ValueObjects;
using GeekShopping.Coupon.Api.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Coupon.Api.Repository;
public class CouponRepository : ICouponRepository
{
    private readonly MySqlContext _context;
    private readonly IMapper _mapper;

    public CouponRepository(MySqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CouponVO> GetCouponByCouponCode(string couponCode)
    {
        var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);

        return _mapper.Map<CouponVO>(coupon);
    }
}
