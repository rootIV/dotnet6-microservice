using GeekShopping.Order.Api.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Order.Api.Model;

[Table("order_header")]
public class OrderHeader : BaseEntitie
{
    [Column("user_id")]
    public string UserId { get; set; }
    [Column("coupon_code")]
    public string CouponCode { get; set; }
    [Column("purchase_amount")]
    public decimal PurchaseAmount { get; set; }
    [Column("discount_amount")]
    public decimal DiscountAmount { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("purchase_date")]
    public DateTime PurchaseDate { get; set; }
    [Column("order_time")]
    public DateTime OrderTime { get; set; }
    [Column("phone_number")]
    public string Phone { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("card_number")]
    public string CardNumber { get; set; }
    [Column("cvv")]
    public string CVV { get; set; }
    [Column("expiry_month_year")]
    public string ExpiryMonthYear { get; set; }
    [Column("order_total_itens")]
    public int OrderTotalItens { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
    [Column("payment_status")]
    public bool PaymentStatus { get; set; }
}
