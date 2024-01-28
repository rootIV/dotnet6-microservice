using GeekShopping.Cart.Api.Data.ValueObjects;
using GeekShopping.Cart.Api.Messages;
using GeekShopping.Cart.Api.RabbitMQSender;
using GeekShopping.Cart.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Cart.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : Controller
{
    private readonly ICartRepository _cartRepository;
    private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

    public CartController(ICartRepository cartRepository, IRabbitMQMessageSender rabbitMQMessageSender)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
    }

    [HttpGet("find-cart/{userId}")]
    public async Task<ActionResult<CartVO>> FindById(string userId)
    {
        var cart = await _cartRepository.FindCartByUserId(userId);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO cartVo)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(cartVo);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVo)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(cartVo);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpDelete("remove-cart/{userId}")]
    public async Task<ActionResult<CartVO>> RemoveCart(int userId)
    {
        var status = await _cartRepository.RemoveFromCart(userId);

        if (!status)
            return BadRequest();

        return Ok(status);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO cartVo)
    {
        var status = await _cartRepository.ApplyCoupon(cartVo.CartHeader.UserId, cartVo.CartHeader.CouponCode);

        if (!status)
            return NotFound();

        return Ok(status);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
    {
        var status = await _cartRepository.RemoveCoupon(userId);

        if (!status)
            return NotFound();

        return Ok(status);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
    {
        if (vo?.UserId == null)
            return BadRequest();

        var cart = await _cartRepository.FindCartByUserId(vo.UserId);

        if (cart == null)
            return NotFound();

        vo.CartDetails = cart.CartDetails;
        vo.DateTime = DateTime.Now;

        _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

        return Ok(vo);
    }
}
