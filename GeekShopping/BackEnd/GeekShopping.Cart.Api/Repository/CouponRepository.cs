using GeekShopping.Cart.Api.Data.ValueObjects;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.Cart.Api.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _httpClient;

    public CouponRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CouponVO> GetCoupon(string couponCode, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"/api/v1/coupon/{couponCode}");

        var content = await response.Content.ReadAsStringAsync();

        if(response.StatusCode != HttpStatusCode.OK)
        {
            return new CouponVO();
        }

        return JsonSerializer.Deserialize<CouponVO>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
