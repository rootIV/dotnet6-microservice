﻿namespace GeekShopping.Cart.Api.Data.ValueObjects;

public class ProductVO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public string Image_url { get; set; }
}
