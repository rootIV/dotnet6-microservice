using GeekShopping.Cart.Api.Data.ValueObjects;

namespace GeekShopping.Cart.Api.Repository;

public interface ICouponRepository
{
    Task<CouponVO> GetCoupon(string couponCode, string token);
}
