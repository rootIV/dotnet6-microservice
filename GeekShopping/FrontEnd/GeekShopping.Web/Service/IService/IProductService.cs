using GeekShopping.Web.Models;

namespace GeekShopping.Web.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> FindAllProducts(string token);
    Task<ProductViewModel> FindProductsById(long id, string token);
    Task<ProductViewModel> CreateProduct(ProductViewModel model, string token);
    Task<ProductViewModel> UpdateProduct(ProductViewModel model, string token);
    Task<bool> DeleteProductById(long id, string token);
}
