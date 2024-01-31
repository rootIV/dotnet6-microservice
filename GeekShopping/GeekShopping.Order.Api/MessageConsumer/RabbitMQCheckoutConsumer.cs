using GeekShopping.Order.Api.Messages;
using GeekShopping.Order.Api.Model;
using GeekShopping.Order.Api.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Order.Api.MessageConsumer;

public class RabbitMQCheckoutConsumer : BackgroundService
{
    private readonly OrderRepository _orderRepository;
    private readonly IConnection _connection;
    private IModel _channel;

    public RabbitMQCheckoutConsumer(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };

        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (chanel, evt) =>
        {
            var content = Encoding.UTF8.GetString(evt.Body.ToArray());
            
            CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);

            ProcessOrder(vo).GetAwaiter().GetResult();

            _channel.BasicAck(evt.DeliveryTag, false);
        };

        _channel.BasicConsume("checkoutqueue", false, consumer);

        return Task.CompletedTask;
    }

    private async Task ProcessOrder(CheckoutHeaderVO vo)
    {
        OrderHeader order = new()
        {
            UserId = vo.UserId,
            FirstName = vo.FirstName,
            LastName = vo.LastName,
            OrderDetails = new List<OrderDetail>(),
            CardNumber = vo.CardNumber,
            CVV = vo.CVV,
            CouponCode = vo.CouponCode,
            DiscountAmount = vo.DiscountAmount,
            Email = vo.Email,
            ExpiryMonthYear = vo.ExpiryMonthYear,
            OrderTime = DateTime.Now,
            PurchaseAmount = vo.PurchaseAmount,
            Phone = vo.Phone,
            PurchaseDate = vo.DateTime,
            PaymentStatus = false
        };

        foreach (var details in vo.CartDetails)
        {
            OrderDetail detail = new()
            {
                ProductId = details.ProductId,
                ProductName = details.Product.Name,
                ProductPrice = details.Product.Price,
                Count = details.Count
            };

            order.OrderTotalItens += details.Count;
            order.OrderDetails.Add(detail);
        }

        await _orderRepository.AddOrder(order);
    }
}
