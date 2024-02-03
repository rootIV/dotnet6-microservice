using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Order.Api.Model.Context;

public class MySqlContext : DbContext
{
    public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
}
