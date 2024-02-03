using GeekShopping.Order.Api.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Order.Api.Model;

[Table("order_detail")]
public class OrderDetail : BaseEntitie
{
    [Column("order_header_id")]
    public long OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public virtual OrderHeader OrderHeader { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }

    [Column("count")]
    public int Count { get; set; }

    [Column("product_name")]
    public string ProductName { get; set; }

    [Column("product_price")]
    public decimal ProductPrice { get; set; }
}
