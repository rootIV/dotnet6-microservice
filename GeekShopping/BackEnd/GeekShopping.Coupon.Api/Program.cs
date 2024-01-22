using AutoMapper;
using GeekShopping.Coupon.Api.Config;
using GeekShopping.Coupon.Api.Model.Context;
using GeekShopping.Coupon.Api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GeekShopping.Coupon.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

        builder.Services.AddDbContext<MySqlContext>(options => options
            .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 35))));

        IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();

        builder.Services.AddSingleton(mapper);

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<ICouponRepository, CouponRepository>();

        builder.Services.AddControllers();

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:4435";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "geek_shopping");
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.Coupon.Api", Version = "v1" });
            s.EnableAnnotations();
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"Enter 'Bearer' [space] and your token!",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
