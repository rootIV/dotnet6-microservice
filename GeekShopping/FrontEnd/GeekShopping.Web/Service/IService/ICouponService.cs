using GeekShopping.Web.Models;

namespace GeekShopping.Web.Service.IService;

public interface ICouponService
{
    Task<CouponViewModel> GetCoupon(string code, string token);
}
