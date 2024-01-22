using GeekShopping.Cart.Api.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Cart.Api.Model;

[Table("cart_header")]
public class CartHeader : BaseEntitie
{
    [Column("user_id")]
    public string UserId { get; set; }

    [Column("coupon_code")]
    public string CouponCode { get; set; }
}
