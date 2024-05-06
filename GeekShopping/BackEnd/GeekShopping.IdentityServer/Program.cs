using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Config;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

        builder.Services.AddDbContext<MySqlContext>(options => options
            .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 37))));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<MySqlContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseErrorEvents = true;
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential();

        builder.Services.AddScoped<IDbInitializer, DbInitialize>();

        builder.Services.AddScoped<IProfileService, ProfileService>();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        var initializer = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();

        initializer.Initialize();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}