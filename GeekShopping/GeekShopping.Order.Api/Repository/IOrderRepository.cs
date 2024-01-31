using GeekShopping.Order.Api.Model;

namespace GeekShopping.Order.Api.Repository;

public interface IOrderRepository
{
    Task<bool> AddOrder(OrderHeader header);
    Task UpdateOrderPaymentStatus(long orderHeaderId, bool status);
}
