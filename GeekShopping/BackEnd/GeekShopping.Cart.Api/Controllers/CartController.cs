using GeekShopping.Cart.Api.Data.ValueObjects;
using GeekShopping.Cart.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Cart.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : Controller
{
    private readonly ICartRepository _repository;

    public CartController(ICartRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("find-cart/{userId}")]
    public async Task<ActionResult<CartVO>> FindById(string userId)
    {
        var cart = await _repository.FindCartByUserId(userId);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO cartVo)
    {
        var cart = await _repository.SaveOrUpdateCart(cartVo);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVo)
    {
        var cart = await _repository.SaveOrUpdateCart(cartVo);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpDelete("remove-cart/{userId}")]
    public async Task<ActionResult<CartVO>> RemoveCart(int userId)
    {
        var status = await _repository.RemoveFromCart(userId);

        if (!status)
            return BadRequest();

        return Ok(status);
    }
}
