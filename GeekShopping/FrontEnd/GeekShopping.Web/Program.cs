using GeekShopping.Web.Service;
using GeekShopping.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;

namespace GeekShopping.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddHttpClient<IProductService, ProductService>(c =>
            c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

        builder.Services.AddHttpClient<ICartService, CartService>(c =>
            c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartAPI"]));

        builder.Services.AddHttpClient<ICouponService, CouponService>(c =>
            c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"]));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
            .AddCookie("Cookies", options => options.ExpireTimeSpan = TimeSpan.FromMinutes(10))
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClientId = "geek_shopping";
                options.ClientSecret = "comi_a_tal_da_sandrinha";
                options.ResponseType = "code";
                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
                options.Scope.Add("geek_shopping");
                options.SaveTokens = true;
            });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}