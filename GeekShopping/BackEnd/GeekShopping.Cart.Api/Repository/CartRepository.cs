using AutoMapper;
using GeekShopping.Cart.Api.Data.ValueObjects;
using GeekShopping.Cart.Api.Model;
using GeekShopping.Cart.Api.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Cart.Api.Repository;
public class CartRepository : ICartRepository
{
    private readonly MySqlContext _context;
    private readonly IMapper _mapper;

    public CartRepository(MySqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartVO> FindCartByUserId(string userId)
    {
        Model.Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(h => 
                h.UserId == userId)
        };

        cart.CartDetails = _context.CartDetails.Where(h => 
            h.CartHeaderId == cart.CartHeader.Id)
            .Include(d => d.Product);

        return _mapper.Map<CartVO>(cart);
    }

    public async Task<CartVO> SaveOrUpdateCart(CartVO cartVo)
    {
        Model.Cart cart = _mapper.Map<Model.Cart>(cartVo);

        var product = await _context.Products.FirstOrDefaultAsync(p => 
            p.Id == cartVo.CartDetails.FirstOrDefault().ProductId);

        if (product == null)
        {
            _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);

            await _context.SaveChangesAsync();
        }

        var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(h => 
            h.UserId == cart.CartHeader.UserId);

        if (cartHeader == null)
        {
            _context.CartHeaders.Add(cart.CartHeader);

            await _context.SaveChangesAsync();

            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartDetails.FirstOrDefault().Product = null;

            _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());

            await _context.SaveChangesAsync();
        }
        else
        {
            var cartDetail = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(d =>
                d.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                d.CartHeaderId == cartHeader.Id);

            if (cartDetail == null)
            {
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;

                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());

                await _context.SaveChangesAsync();
            }
            else
            {
                cart.CartDetails.FirstOrDefault().Product = null;
                cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());

                await _context.SaveChangesAsync();
            }
        }

        return _mapper.Map<CartVO>(cart);
    }

    public async Task<bool> RemoveFromCart(long cartDetailsId)
    {
        try
        {
            CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(d =>
                d.Id == cartDetailsId);

            //If any error appear relate to count, here is
            int total = _context.CartDetails.Count(d => d.CartHeaderId == cartDetail.CartHeaderId);

            _context.CartDetails.Remove(cartDetail);

            if (total == 1)
            {
                var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(d => 
                    d.Id == cartDetail.CartHeaderId);

                _context.CartHeaders.Remove(cartHeaderToRemove);
            }

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(d =>
            d.UserId == userId);

        if (cartHeader != null)
        {
            _context.CartDetails.RemoveRange(_context.CartDetails.Where(d => 
                d.CartHeaderId == cartHeader.Id));

            _context.CartHeaders.Remove(cartHeader);

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        var header = await _context.CartHeaders.FirstOrDefaultAsync(d =>
            d.UserId == userId);

        if (header != null)
        {
            header.CouponCode = couponCode;

            _context.CartHeaders.Update(header);

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        var header = await _context.CartHeaders.FirstOrDefaultAsync(d =>
            d.UserId == userId);

        if (header != null)
        {
            header.CouponCode = "";

            _context.CartHeaders.Update(header);

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }
}
