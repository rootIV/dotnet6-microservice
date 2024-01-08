﻿using GeekShopping.Web.Models;
using GeekShopping.Web.Service.IService;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
    private IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [Authorize]
    public async Task<IActionResult> ProductIndex()
    {
            var token = await HttpContext.GetTokenAsync("access_token");

            return View(await _productService.FindAllProducts(token));
    }

    public async Task<IActionResult> ProductCreate()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.CreateProduct(productModel, token);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));
        }

        return View(productModel);
    }

    public async Task<IActionResult> ProductUpdate(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.FindProductsById(id, token);

        if (response != null)
            return View(response);

        return NotFound();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductModel productModel)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.UpdateProduct(productModel, token);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));
        }

        return View(productModel);
    }

    public async Task<IActionResult> ProductDelete(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.FindProductsById(id, token);

        if (response != null)
            return View(response);

        return NotFound();
    }
    [Authorize(Roles = Role.Admin)]
    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductModel productModel)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.DeleteProductById(productModel.Id, token);

        if (response)
            return RedirectToAction(nameof(ProductIndex));

        return View(productModel);
    }
}