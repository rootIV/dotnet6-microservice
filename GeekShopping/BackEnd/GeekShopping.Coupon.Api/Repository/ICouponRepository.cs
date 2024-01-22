using GeekShopping.Coupon.Api.Data.ValueObjects;

namespace GeekShopping.Coupon.Api.Repository;

public interface ICouponRepository
{
    Task<CouponVO> GetCouponByCouponCode(string couponCode);
}
