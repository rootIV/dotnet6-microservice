using GeekShopping.Web.Models;
using GeekShopping.Web.Service.IService;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;
using System.Net;

namespace GeekShopping.Web.Service;

public class CouponService : ICouponService
{
    private readonly HttpClient _httpClient;
    public const string BasePath = "api/v1/coupon";

    public CouponService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentException(null, nameof(httpClient));
    }

    public async Task<CouponViewModel> GetCoupon(string code, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"{BasePath}/{code}");

        if (response.StatusCode != HttpStatusCode.OK)
            return new CouponViewModel();

        return await response.ReadContentAs<CouponViewModel>();
    }
}
