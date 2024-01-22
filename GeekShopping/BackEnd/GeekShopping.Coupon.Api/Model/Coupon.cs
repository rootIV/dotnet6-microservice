using GeekShopping.Coupon.Api.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Coupon.Api.Model;

[Table("coupon")]
public class Coupon : BaseEntitie
{
    [Column("coupon_code")]
    [Required]
    [StringLength(30)]
    public string CouponCode { get; set; }

    [Column("discount_amount")]
    [Required]
    public decimal DiscountAmount { get; set; }
}
