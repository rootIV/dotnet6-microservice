﻿using GeekShopping.Product.Api.Data.ValueObjects;

namespace GeekShopping.Product.Api.Repository;

public interface IProductRepository
{
    Task<IEnumerable<ProductVO>> FindAll();
    Task<ProductVO> FindById(long id);
    Task<ProductVO> Create(ProductVO vo);
    Task<ProductVO> Update(ProductVO vo);
    Task<bool> Delete(long id);
}
