using GeekShopping.Web.Models;
using GeekShopping.Web.Service.IService;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Service;

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;
    public const string BasePath = "api/v1/cart";

    public CartService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentException(null, nameof(httpClient));
    }

    public async Task<CartViewModel> FindCartByUserId(string userId, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"{BasePath}/find-cart/{userId}");

        return await response.ReadContentAs<CartViewModel>();
    }

    public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsJsonAsync($"{BasePath}/add-cart", cart);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        else
            throw new Exception("Something went worng when calling API");
    }

    public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsJsonAsync($"{BasePath}/update-cart", cart);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        else
            throw new Exception("Something went worng when calling API");
    }

    public async Task<bool> RemoveFromCart(long cartId, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"{BasePath}/remove-cart/{cartId}");

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else
            throw new Exception("Something went worng when calling API");
    }

    public async Task<bool> ClearCart(string userId, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ApplyCoupon(CartViewModel cart, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsJsonAsync($"{BasePath}/apply-coupon", cart);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else
            throw new Exception("Something went worng when calling API");
    }

    public async Task<bool> RemoveCoupon(string userId, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"{BasePath}/remove-coupon/{userId}");

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else
            throw new Exception("Something went worng when calling API");
    }

    public async Task<CartHeaderViewModel> Checkout(CartHeaderViewModel cartHeader, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsJsonAsync($"{BasePath}/checkout", cartHeader);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartHeaderViewModel>();
        else
            throw new Exception("Something went worng when calling API");
    }
}
