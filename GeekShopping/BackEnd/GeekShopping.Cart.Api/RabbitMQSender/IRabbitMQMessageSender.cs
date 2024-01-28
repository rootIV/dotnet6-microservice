using GeekShopping.MessageBus;

namespace GeekShopping.Cart.Api.RabbitMQSender;

public interface IRabbitMQMessageSender
{
    void SendMessage(BaseMessage message, string queueName);
}
