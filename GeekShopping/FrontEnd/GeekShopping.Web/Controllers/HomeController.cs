using GeekShopping.Web.Models;
using GeekShopping.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekShopping.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, 
        IProductService productService,
        ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index() =>
        View(await _productService.FindAllProducts(""));

    [Authorize]
    public async Task<IActionResult> Details(int id) =>
        View(await _productService.FindProductsById(id, await HttpContext.GetTokenAsync("access_token")));

    [Authorize]
    [ActionName("Details")]
    [HttpPost]
    public async Task<IActionResult> DetailsPost(ProductViewModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        CartViewModel cart = new()
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailViewModel cartDetail = new()
        {
            Count = model.Count,
            ProductId = model.Id,
            Product = await _productService.FindProductsById(model.Id, token)
        };

        List<CartDetailViewModel> cartDetails = new() { cartDetail };

        cart.CartDetails = cartDetails;

        var response = await _cartService.AddItemToCart(cart, token);

        if (response != null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public IActionResult Privacy() =>
        View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => 
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [Authorize]
    public IActionResult Login() =>
        RedirectToAction(nameof(Index));

    public IActionResult Logout() =>
        SignOut("Cookies", "oidc");
}
