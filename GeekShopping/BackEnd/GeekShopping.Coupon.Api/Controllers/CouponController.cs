using GeekShopping.Coupon.Api.Data.ValueObjects;
using GeekShopping.Coupon.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Coupon.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository _repository;

    public CouponController(ICouponRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("{couponCode}")]
    [Authorize]
    public async Task<ActionResult<CouponVO>> FindById(string couponCode)
    {
        var coupon = await _repository.GetCouponByCouponCode(couponCode);

        if (coupon == null)
            return NotFound();

        return Ok(coupon);
    }
}
