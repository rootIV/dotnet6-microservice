using GeekShopping.Web.Models;
using GeekShopping.Web.Service.IService;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Service;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    public const string BasePath = "api/v1/product";

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
    }

    public async Task<IEnumerable<ProductModel>> FindAllProducts()
    {
        var response = await _httpClient.GetAsync(BasePath);

        return await response.ReadContentAs<List<ProductModel>>();
    }
    public async Task<ProductModel> FindProductsById(long id)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}");

        return await response.ReadContentAs<ProductModel>();
    }
    public async Task<ProductModel> CreateProduct(ProductModel model)
    {
        var response = await _httpClient.PostAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductModel>();
        else
            throw new Exception("Something went worng when calling API");
    }
    public async Task<ProductModel> UpdateProduct(ProductModel model)
    {
        var response = await _httpClient.PutAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductModel>();
        else
            throw new Exception("Something went worng when calling API");
    }
    public async Task<bool> DeleteProductById(long id)
    {
        var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else
            throw new Exception("Something went worng when calling API");
    }
}
