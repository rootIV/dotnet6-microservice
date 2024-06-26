using GeekShopping.Order.Api.MessageConsumer;
using GeekShopping.Order.Api.Model.Context;
using GeekShopping.Order.Api.RabbitMQSender;
using GeekShopping.Order.Api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GeekShopping.Order.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

        builder.Services.AddDbContext<MySqlContext>(options => options
            .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 37))));

        var contextBuilder = new DbContextOptionsBuilder<MySqlContext>();

        contextBuilder.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)));

        builder.Services.AddSingleton(new OrderRepository(contextBuilder.Options));

        builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();
        builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

        builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

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
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.Order.Api", Version = "v1" });
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
