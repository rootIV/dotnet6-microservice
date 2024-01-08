﻿using GeekShopping.Web.Models;

namespace GeekShopping.Web.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> FindAllProducts(string token);
    Task<ProductModel> FindProductsById(long id, string token);
    Task<ProductModel> CreateProduct(ProductModel model, string token);
    Task<ProductModel> UpdateProduct(ProductModel model, string token);
    Task<bool> DeleteProductById(long id, string token);
}
