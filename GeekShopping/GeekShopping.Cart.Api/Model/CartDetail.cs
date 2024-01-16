using GeekShopping.Cart.Api.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Cart.Api.Model;

[Table("cart_detail")]
public class CartDetail : BaseEntitie
{
    [Column("cart_header_id")]
    public long CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")]
    public CartHeader CartHeader { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    [Column("count")]
    public int Count { get; set; }
}
